using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace WbApiData
{
    public class WbClient
    {
        private const string LogisticUrl = "https://common-api.wildberries.ru/api/v1/tariffs/box";
        private const string CardsInfoUrl = "https://content-api.wildberries.ru/content/v2/get/cards/list";
        private const string StocksUrl = "https://statistics-api.wildberries.ru/api/v1/supplier/stocks";
        private const string SalesUrl = "https://statistics-api.wildberries.ru/api/v1/supplier/sales";
        private const string OrdersUrl = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";
        private const string StatistucUrl = "https://seller-analytics-api.wildberries.ru/api/analytics/v3/sales-funnel/products";
        private const string CompaniesInfoUrl = "https://advert-api.wildberries.ru/adv/v1/promotion/adverts";
        private const string CompaniesUrl = "https://advert-api.wildberries.ru/adv/v1/promotion/count";
        private const string CompaniesStatUrl = "https://advert-api.wildberries.ru/adv/v3/fullstats";
        private const string PriceAndDiscountUrl = "https://discounts-prices-api.wildberries.ru/api/v2/list/goods/filter";
        private const string WarehouseRemainsUrl = "https://seller-analytics-api.wildberries.ru/api/v1/warehouse_remains";
        private const string PaidStorageUrl = "https://seller-analytics-api.wildberries.ru/api/v1/paid_storage";
        private const string CategoriesUrl = "https://content-api.wildberries.ru/content/v2/object/all?limit=1000";
        private const string CommissionsUrl = "https://common-api.wildberries.ru/api/v1/tariffs/commission";
        private const string CostHistoryUrl = "https://advert-api.wildberries.ru/adv/v1/upd";
        private const string FinancialReportUrl = "https://statistics-api.wildberries.ru/api/v5/supplier/reportDetailByPeriod";
        private const string WarehouseRemainsCreateReportEnding = "groupBySa=true&groupByNm=true&groupByBarcode=true&groupBySize=true";
        private const string CheckStatusEnding = "status";
        private const string DownloadEnding = "download";

        private readonly HttpClient _client;

        public WbClient()
        {
            _client = new()
            {
                Timeout = new TimeSpan(0, 2, 0)
            };
        }

        public void ChangeApiKey(string apiKey)
        {
            try
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("Authorization", apiKey);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in ChangeApiKey:\n" + exception);
            }
        }

        public async Task<bool> IsKeyValidate()
        {
            try
            {
                string url = $"{CompaniesUrl}";
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.Unauthorized ||
                    response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"\n\nОшибка в IsKeyValidate\n{exception}\n\n");
                return false;
            }
        }

        public async Task<Dictionary<int, Card>> GetCards()
        {
            try
            {
                Dictionary<int, Card> articuleToGoodInfo = [];
                bool hasMore = true;
                string? updatedAt = null;
                int nmID = 0;

                while (hasMore)
                {
                    var requestBody = new
                    {
                        settings = new
                        {
                            cursor = new
                            {
                                limit = 100,
                                updatedAt,
                                nmID
                            },
                            filter = new
                            {
                                withPhoto = -1
                            }
                        }
                    };

                    string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PostAsync(CardsInfoUrl, content);

                    if (response.IsSuccessStatusCode == false)
                    {
                        Console.WriteLine("Ошибка запроса карточек товара: " + await response.Content.ReadAsStringAsync());
                        break;
                    }

                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument data = JsonDocument.Parse(result);
                    JsonElement cards = data.RootElement.GetProperty("cards");
                    JsonElement cursor = data.RootElement.GetProperty("cursor");

                    foreach (JsonElement card in cards.EnumerateArray())
                    {
                        int articule = card.GetProperty("nmID").GetInt32();

                        articuleToGoodInfo[articule] = new Card(
                            card.GetProperty("subjectID").GetInt32(),
                            card.GetProperty("subjectName").ToString(),
                            card.GetProperty("brand").GetString()!,
                            articule,
                            card.GetProperty("vendorCode").GetString()!,
                            card.TryGetProperty("photos", out JsonElement photos) ? photos.EnumerateArray().First().TryGetProperty("big", out JsonElement bigPhoto) ? bigPhoto.GetString()! : "No photo" : "No photo",
                            card.GetProperty("dimensions").GetProperty("length").GetInt32(),
                            card.GetProperty("dimensions").GetProperty("width").GetInt32(),
                            card.GetProperty("dimensions").GetProperty("height").GetInt32(),
                            card.GetProperty("dimensions").GetProperty("weightBrutto").GetDouble(),
                            card.GetProperty("sizes").EnumerateArray().First().GetProperty("techSize").GetString()!,
                            card.GetProperty("sizes").EnumerateArray().First().GetProperty("skus").EnumerateArray().First().GetString()!);
                    }

                    updatedAt = cursor.GetProperty("updatedAt").GetString();
                    nmID = cursor.GetProperty("nmID").GetInt32();
                    hasMore = cursor.GetProperty("total").GetInt32() >= 100;

                    await Task.Delay(1000);
                }

                return articuleToGoodInfo;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetCards:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, Stock>> GetFullStocks()
        {
            try
            {
                Dictionary<int, Stock> articuleToStock = [];

                HttpResponseMessage taskIdResponse = await _client.GetAsync(WarehouseRemainsUrl + "?" + WarehouseRemainsCreateReportEnding);
                string taskIdResult = await taskIdResponse.Content.ReadAsStringAsync();
                JsonDocument taskIdDocument = JsonDocument.Parse(taskIdResult);
                JsonElement taskIdData = taskIdDocument.RootElement.GetProperty("data");

                string warehouseRemainsUrl = WarehouseRemainsUrl + "/" + "tasks/" + taskIdData.GetProperty("taskId") + "/";

                while (true)
                {
                    HttpResponseMessage statusResponse = await _client.GetAsync(warehouseRemainsUrl + CheckStatusEnding);
                    string statusResult = await statusResponse.Content.ReadAsStringAsync();
                    JsonDocument statusDocument = JsonDocument.Parse(statusResult);
                    JsonElement statusData = statusDocument.RootElement.GetProperty("data");

                    if (statusData.GetProperty("status").ToString() == "done")
                    {
                        break;
                    }

                    await Task.Delay(6000);
                }

                HttpResponseMessage stocksResponse = await _client.GetAsync(warehouseRemainsUrl + DownloadEnding);
                stocksResponse.EnsureSuccessStatusCode();
                string stocksResult = await stocksResponse.Content.ReadAsStringAsync();

                List<Stock>? stocks = JsonConvert.DeserializeObject<List<Stock>>(stocksResult);

                foreach (Stock stock in stocks!)
                {
                    articuleToStock[stock.Articule] = stock;
                }

                return articuleToStock;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception in {nameof(GetFullStocks)}:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, StockReport>> GetStocks(DateTime todayDate)
        {
            try
            {
                Dictionary<int, StockReport> articulesToStock = [];

                HttpResponseMessage response = await _client.GetAsync($"{StocksUrl}?dateFrom={todayDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    List<StockReport> stocks = JsonConvert.DeserializeObject<List<StockReport>>(result)!;

                    foreach (StockReport stock in stocks)
                    {
                        articulesToStock[stock.NmId] = stock;
                    }

                    return articulesToStock;
                }
                else
                {
                    Console.WriteLine("Ошибка запроса остатков: " + await response.Content.ReadAsStringAsync());
                    return [];
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetStocks:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, SaleReport>> GetSales(DateTime todayDate)
        {
            try
            {
                Dictionary<int, SaleReport> articuleToSale = [];

                HttpResponseMessage response = await _client.GetAsync($"{SalesUrl}?dateFrom={todayDate:yyyy-MM-dd}&flag=1");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    List<SaleReport> sales = JsonConvert.DeserializeObject<List<SaleReport>>(result)!;

                    foreach (SaleReport sale in sales)
                    {
                        int articule = sale.NmId;

                        if (articuleToSale.TryGetValue(articule, out SaleReport? existingSale))
                            existingSale.MergeWith(sale);
                        else
                            articuleToSale[articule] = sale;
                    }

                    return articuleToSale;
                }
                else
                {
                    Console.WriteLine("Ошибка запроса продаж: " + await response.Content.ReadAsStringAsync());
                    return [];
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetSales:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, OrdersReport>> GetOrders(DateTime todayDate)
        {
            try
            {
                Dictionary<int, OrdersReport> articuleToOrders = [];

                HttpResponseMessage response = await _client.GetAsync($"{OrdersUrl}?dateFrom={todayDate:yyyy-MM-dd}&flag=1");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    List<OrdersReport> orders = JsonConvert.DeserializeObject<List<OrdersReport>>(result)!;

                    foreach (OrdersReport order in orders)
                    {
                        int articule = order.NmId;

                        if (articuleToOrders.TryGetValue(articule, out OrdersReport? existingOrder))
                            existingOrder.MergeWith(order);
                        else
                            articuleToOrders[articule] = order;
                    }
                }

                return articuleToOrders;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetSales:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, Statistic>> GetStatistics(DateTime todayDate)
        {
            try
            {
                Dictionary<int, Statistic> articuleToStatistic = [];

                var requestBody = new
                {
                    selectedPeriod = new
                    {
                        start = $"{todayDate:yyyy-MM-dd}",
                        end = $"{todayDate:yyyy-MM-dd}"
                    },
                    limit = 1000
                };

                string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(StatistucUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);

                    JsonElement data = document.RootElement.GetProperty("data");
                    JsonElement products = data.GetProperty("products");

                    List<Statistic> statistics = JsonConvert.DeserializeObject<List<Statistic>>(products.ToString())!;

                    foreach (Statistic statistic in statistics)
                    {
                        articuleToStatistic[statistic.Product.NmId] = statistic;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка запроса статистики");
                    return [];
                }

                return articuleToStatistic;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetStatistics:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, Statistic>> GetStatisticsForMonth(DateTime todayDate)
        {
            try
            {
                Dictionary<int, Statistic> articuleToStatistic = [];

                var requestBody = new
                {
                    period = new
                    {
                        begin = $"{todayDate.AddDays(-29):yyyy-MM-dd} 00:00:00",
                        end = $"{todayDate:yyyy-MM-dd} 23:59:59"
                    },
                    limit = 1000
                };

                string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(StatistucUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);

                    JsonElement data = document.RootElement.GetProperty("data");
                    JsonElement products = data.GetProperty("products");

                    List<Statistic> statistics = JsonConvert.DeserializeObject<List<Statistic>>(products.ToString())!;

                    foreach (Statistic statistic in statistics)
                    {
                        articuleToStatistic[statistic.Product.NmId] = statistic;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка запроса статистики");
                    return [];
                }

                return articuleToStatistic;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetStatistics:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, PriceAndDiscount>> GetPrice()
        {
            try
            {
                Dictionary<int, PriceAndDiscount> articuleToPriceAndDiscount = [];
                int limit = 1000;
                int offset = 0;
                JsonElement.ArrayEnumerator listGoods = [];

                do
                {
                    string currentResponseUrl = $"{PriceAndDiscountUrl}?limit={limit}&offset={offset}";
                    HttpResponseMessage response = await _client.GetAsync(currentResponseUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        JsonDocument document = JsonDocument.Parse(result);
                        JsonElement data = document.RootElement.GetProperty("data");
                        listGoods = data.GetProperty("listGoods").EnumerateArray();

                        foreach (JsonElement good in listGoods)
                        {
                            int articule = good.GetProperty("nmID").GetInt32();

                            articuleToPriceAndDiscount[articule] = new PriceAndDiscount(articule,
                                good.GetProperty("vendorCode").GetString()!,
                                good.GetProperty("sizes").EnumerateArray().First().GetProperty("price").GetInt32(),
                                good.GetProperty("discount").GetInt32(),
                                good.GetProperty("sizes").EnumerateArray().First().GetProperty("discountedPrice").GetDouble());
                        }

                        offset += limit;
                    }
                } while (listGoods.Any());

                return articuleToPriceAndDiscount;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetPrice:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, ArticulesCompanyStat>> GetCompanyToArticulesStat(List<int> companyIds, DateTime todayDate)
        {
            if (companyIds.Count == 0)
                return [];

            const int maxPerBatch = 99;

            Dictionary<int, ArticulesCompanyStat> companyIdToAritculesStat = [];

            try
            {
                List<List<int>> splitCompanyIds = [];
                string idsParam;
                string urlResponse;

                if (companyIds.Count > 0)
                {
                    for (int i = 0; i < companyIds.Count; i += maxPerBatch)
                    {
                        List<int> chunk = companyIds
                            .Skip(i)
                            .Take(Math.Min(maxPerBatch, companyIds.Count - i))
                            .ToList();

                        splitCompanyIds.Add(chunk);
                    }
                }

                foreach (List<int> companyIdsPart in splitCompanyIds)
                {
                    idsParam = string.Join(",", companyIdsPart);
                    urlResponse = $"{CompaniesStatUrl}?ids={idsParam}&beginDate={todayDate:yyyy-MM-dd}&endDate={todayDate:yyyy-MM-dd}";

                    HttpResponseMessage response = await _client.GetAsync(urlResponse);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        List<ArticulesCompanyStat> articulesCompanyStats = JsonConvert.DeserializeObject<List<ArticulesCompanyStat>>(result)!;

                        foreach (ArticulesCompanyStat articulesCompanyStat in articulesCompanyStats)
                        {
                            companyIdToAritculesStat[articulesCompanyStat.AdvertId] = articulesCompanyStat;
                        }
                    }
                    else
                    {
                        string errorBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Ошибка запроса articules companies stat ({response.StatusCode}): {errorBody}");
                    }

                    await Task.Delay(60000);
                }

                return companyIdToAritculesStat;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Ошибка получения marketing stat - " + exception);
                return companyIdToAritculesStat;
            }
        }

        public async Task<List<int>> GetCompaniesByStatus(List<CompanyStatus> companiesStatus)
        {
            try
            {
                List<int> companiesId = [];
                HttpResponseMessage response = await _client.GetAsync(CompaniesUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);

                    foreach (JsonElement advert in document.RootElement.GetProperty("adverts").EnumerateArray())
                    {
                        int statusValue = advert.GetProperty("status").GetInt32();

                        if (Enum.IsDefined(typeof(CompanyStatus), statusValue) == false)
                        {
                            Console.WriteLine("Ошибка получения enum status");
                            continue;
                        }

                        CompanyStatus status = (CompanyStatus)statusValue;

                        if (companiesStatus.Contains(status) == false)
                            continue;

                        foreach (JsonElement company in advert.GetProperty("advert_list").EnumerateArray())
                        {
                            companiesId.Add(company.GetProperty("advertId").GetInt32());
                        }
                    }
                }
                else
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка запроса GetCompaniesByStatus ({response.StatusCode}): {errorBody}");
                }

                return companiesId;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetCompaniesByStatus:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, Company>> GetCompaniesInfo(List<int> companyIds)
        {
            try
            {
                Dictionary<int, Company> advertIdToCompanyInfo = [];

                if (companyIds.Count == 0)
                {
                    return [];
                }

                string jsonContent = System.Text.Json.JsonSerializer.Serialize(companyIds);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync($"{CompaniesInfoUrl}", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(result)!;

                    foreach (Company company in companies)
                    {
                        advertIdToCompanyInfo[company.AdvertId] = company;
                    }

                    return advertIdToCompanyInfo;
                }
                else
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка запроса GetCompaniesInfo ({response.StatusCode}): {errorBody}");
                    return [];
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetCompaniesInfo:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<string, Logistic>> GetLogistic(DateTime todayDate)
        {
            try
            {
                Dictionary<string, Logistic> warehouseNameToLogistic = [];

                HttpResponseMessage response = await _client.GetAsync($"{LogisticUrl}?date={todayDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);

                    List<Logistic> logistics = JsonConvert.DeserializeObject<List<Logistic>>(document.RootElement.GetProperty("response").GetProperty("data").GetProperty("warehouseList").ToString())!;

                    foreach (Logistic logistic in logistics)
                    {
                        warehouseNameToLogistic[logistic.WarehouseName] = logistic;
                    }
                }

                return warehouseNameToLogistic;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetLogistic:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, PaidStorage>> GetPaidStorage(DateTime todayDate)
        {
            try
            {
                Dictionary<int, PaidStorage> articuleToPaidStorage = [];

                HttpResponseMessage taskIdResponse = await _client.GetAsync($"{PaidStorageUrl}?dateFrom={todayDate:yyyy-MM-dd}T00:00:00&dateTo={todayDate:yyyy-MM-dd}T23:59:59");
                string taskIdResult = await taskIdResponse.Content.ReadAsStringAsync();
                JsonDocument taskIdDocument = JsonDocument.Parse(taskIdResult);
                JsonElement taskIdData = taskIdDocument.RootElement.GetProperty("data");

                string taskId = taskIdData.GetProperty("taskId").GetString()!;

                bool status = false;

                while (status == false)
                {
                    HttpResponseMessage taskCheckTaskIdStatusResponse = await _client.GetAsync($"{PaidStorageUrl}/tasks/{taskId}/{CheckStatusEnding}");
                    string taskCheckTaskIdStatusResult = await taskCheckTaskIdStatusResponse.Content.ReadAsStringAsync();
                    JsonDocument taskCheckTaskIdStatusDocument = JsonDocument.Parse(taskCheckTaskIdStatusResult);
                    JsonElement taskCheckTaskIdStatusData = taskCheckTaskIdStatusDocument.RootElement.GetProperty("data");

                    status = taskCheckTaskIdStatusData.GetProperty("status").GetString() == "done";

                    await Task.Delay(5000);
                }

                HttpResponseMessage reportResponse = await _client.GetAsync($"{PaidStorageUrl}/tasks/{taskId}/{DownloadEnding}");
                string reportResult = await reportResponse.Content.ReadAsStringAsync();
                List<PaidStorage> paids = JsonConvert.DeserializeObject<List<PaidStorage>>(reportResult)!;

                int count = 0;

                foreach (var paid in paids)
                {
                    count++;

                    int articule = paid.NmId;

                    if (articuleToPaidStorage.TryGetValue(articule, out PaidStorage? paidStorage))
                    {
                        paidStorage.MergeWith(paid);
                    }
                    else
                    {
                        paid.GetStartPrice(paid.KeepingPrice);
                        articuleToPaidStorage[articule] = paid;
                    }
                }

                return articuleToPaidStorage;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetPaidStorage:\n" + exception);
                return [];
            }
        }

        public async Task<List<Categorie>> GetCategories()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(CategoriesUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);
                    JsonElement data = document.RootElement.GetProperty("data");

                    return JsonConvert.DeserializeObject<List<Categorie>>(data.ToString()!)!;
                }
                else
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка запроса GetCategories ({response.StatusCode}): {errorBody}");
                    return [];
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetCategories:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, Commissions>> GetCommisions()
        {
            try
            {
                Dictionary<int, Commissions> subjectIDToCommissions = [];

                HttpResponseMessage response = await _client.GetAsync(CommissionsUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);
                    JsonElement report = document.RootElement.GetProperty("report");

                    List<Commissions> commissions = JsonConvert.DeserializeObject<List<Commissions>>(report.ToString())!;

                    foreach (Commissions commission in commissions)
                    {
                        subjectIDToCommissions[commission.SubjectID] = commission;
                    }

                    return subjectIDToCommissions;
                }
                else
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка запроса GetCommisions ({response.StatusCode}): {errorBody}");
                    return [];
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetCommisions:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, CostHistory>> GetCostHistory(DateTime todayDate)
        {
            try
            {
                Dictionary<int, CostHistory> advertIdToCostHistory = [];

                HttpResponseMessage response = await _client.GetAsync($"{CostHistoryUrl}?from={todayDate:yyyy-MM-dd}&to={todayDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    List<CostHistory> costHistories = JsonConvert.DeserializeObject<List<CostHistory>>(result)!;

                    foreach (CostHistory costHistory in costHistories)
                    {
                        advertIdToCostHistory[costHistory.AdvertId] = costHistory;
                    }

                    return advertIdToCostHistory;
                }
                else
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка запроса GetCostHistory ({response.StatusCode}): {errorBody}");
                    return [];
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in GetCostHistory:\n" + exception);
                return [];
            }
        }

        public async Task<Dictionary<int, FinancialReport>> GetFinancialReport(DateTime todayDate)
        {
            try
            {
                int DaysCountInWeek = 6;

                Dictionary<int, FinancialReport> articuleToFinancialReport = [];

                HttpResponseMessage response = await _client.GetAsync($"{FinancialReportUrl}?dateFrom={todayDate.AddDays(-DaysCountInWeek):yyyy-MM-dd}T00:00:00&dateTo={todayDate:yyyy-MM-dd}T23:59:59");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    List<FinancialReport> FinancialReports = JsonConvert.DeserializeObject<List<FinancialReport>>(result)!;

                    foreach (FinancialReport financialReport in FinancialReports)
                    {
                        articuleToFinancialReport[financialReport.NmId] = financialReport;
                    }

                    return articuleToFinancialReport;
                }
                else
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка запроса GetCostHistory ({response.StatusCode}): {errorBody}");
                    return [];
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception in {nameof(GetFinancialReport)}:\n" + exception);
                return [];
            }
        }
    }
}