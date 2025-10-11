using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace WbApiData
{
    public class WbClient
    {
        private const string LogisticUrl = "https://common-api.wildberries.ru/api/v1/tariffs/box";
        private const string FullCardsInfoUrl = "https://seller-analytics-api.wildberries.ru/api/v2/search-report/report";
        private const string CardsInfoUrl = "https://content-api.wildberries.ru/content/v2/get/cards/list";
        private const string SalesUrl = "https://statistics-api.wildberries.ru/api/v1/supplier/sales";
        private const string StatistucUrl = "https://seller-analytics-api.wildberries.ru/api/v2/nm-report/detail";
        private const string CompaniesURL = "https://advert-api.wildberries.ru/adv/v1/promotion/count";
        private const string CompaniesStatURL = "https://advert-api.wildberries.ru/adv/v3/fullstats";
        private const string PriceAndDiscountUrl = "https://discounts-prices-api.wildberries.ru/api/v2/list/goods/filter";
        private const string WarehouseRemainsUrl = "https://seller-analytics-api.wildberries.ru/api/v1/warehouse_remains";
        private const string WarehouseRemainsCreateReportEnding = "groupBySa=true&groupByNm=true&groupByBarcode=true&groupBySize=true";
        private const string WarehouseRemainsCheckStatusEnding = "status";
        private const string WarehouseRemainsDownloadEnding = "download";

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
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", apiKey);
        }

        public async Task<bool> IsKeyValidate()
        {
            try
            {
                string url = $"{CompaniesURL}";
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
                    Console.WriteLine("Ошибка запроса: " + await response.Content.ReadAsStringAsync());
                    break;
                }

                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<CardsResponse>(result);

                if (data is null) break;

                foreach (var card in data.Cards)
                {
                    articuleToGoodInfo[card.Articule] = card;
                }

                hasMore = data.Cursor.Total >= 100;
                updatedAt = data.Cursor.UpdatedAt;
                nmID = data.Cursor.NmID;

                await Task.Delay(1000);
            }

            return articuleToGoodInfo;
        }

        public async Task<Dictionary<int, Stock>> GetStocks()
        {
            Dictionary<int, Stock> articuleToStock = [];

            HttpResponseMessage taskIdResponse = await _client.GetAsync(WarehouseRemainsUrl + "?" + WarehouseRemainsCreateReportEnding);
            string taskIdResult = await taskIdResponse.Content.ReadAsStringAsync();
            JsonDocument taskIdDocument = JsonDocument.Parse(taskIdResult);
            JsonElement taskIdData = taskIdDocument.RootElement.GetProperty("data");

            string warehouseRemainsUrl = WarehouseRemainsUrl + "/" + "tasks/" + taskIdData.GetProperty("taskId") + "/";

            while (true)
            {
                HttpResponseMessage statusResponse = await _client.GetAsync(warehouseRemainsUrl + WarehouseRemainsCheckStatusEnding);
                string statusResult = await statusResponse.Content.ReadAsStringAsync();
                JsonDocument statusDocument = JsonDocument.Parse(statusResult);
                JsonElement statusData = statusDocument.RootElement.GetProperty("data");

                if (statusData.GetProperty("status").ToString() == "done")
                {
                    break;
                }

                await Task.Delay(6000);
            }

            HttpResponseMessage stocksResponse = await _client.GetAsync(warehouseRemainsUrl + WarehouseRemainsDownloadEnding);
            stocksResponse.EnsureSuccessStatusCode();
            string stocksResult = await stocksResponse.Content.ReadAsStringAsync();

            List<Stock>? stocks = JsonConvert.DeserializeObject<List<Stock>>(stocksResult);

            foreach (Stock stock in stocks!)
            {
                articuleToStock[stock.Articule] = stock;
            }

            return articuleToStock;
        }

        public async Task<Dictionary<int, Sale>> GetSales(DateTime todayDate)
        {
            Dictionary<int, Sale> articuleToSale = [];

            HttpResponseMessage response = await _client.GetAsync($"{SalesUrl}?dateFrom={todayDate:yyyy-MM-dd}&flag=1");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(result)!;

                foreach (Sale sale in sales)
                {
                    int articule = sale.Articule;

                    if (articuleToSale.TryGetValue(articule, out Sale? value))
                    {
                        value.IncreaseQuantity();
                    }
                    else
                    {
                        articuleToSale[articule] = sale;
                    }
                }
            }

            return articuleToSale;
        }

        public async Task<Dictionary<int, Statistic>> GetStatistics(DateTime todayDate)
        {
            Dictionary<int, Statistic> articuleToStatistic = [];
            bool isNextPage = true;
            int page = 1;

            while (isNextPage)
            {
                var requestBody = new
                {
                    period = new
                    {
                        begin = $"{todayDate:yyyy-MM-dd} 00:00:00",
                        end = $"{todayDate:yyyy-MM-dd} 23:59:59"
                    },
                    page
                };

                string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(StatistucUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);

                    JsonElement data = document.RootElement.GetProperty("data");
                    JsonElement cards = data.GetProperty("cards");
                    isNextPage = data.GetProperty("isNextPage").GetBoolean();
                    page++;

                    List<Statistic> statistics = JsonConvert.DeserializeObject<List<Statistic>>(cards.ToString())!;

                    foreach (Statistic statistic in statistics)
                    {
                        articuleToStatistic[statistic.Articule] = statistic;
                    }

                    await Task.Delay(20000);
                }
                else
                {
                    Console.WriteLine("Ошибка зароса статистики");
                }
            }

            return articuleToStatistic;
        }

        public async Task<Dictionary<int, PriceAndDiscount>> GetPriceAndDiscount()
        {
            Dictionary<int, PriceAndDiscount> articuleToPriceAndDiscount = [];
            int limit = 1000;
            int offset = 0;

            while (true)
            {
                string currentResponseUrl = $"{PriceAndDiscountUrl}?limit={limit}&offset={offset}";
                HttpResponseMessage response = await _client.GetAsync(currentResponseUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);
                    JsonElement data = document.RootElement.GetProperty("data");
                    JsonElement listGoods = data.GetProperty("listGoods");

                    List<PriceAndDiscount> pricesAndDiscounts = JsonConvert.DeserializeObject<List<PriceAndDiscount>>(listGoods.ToString())!;

                    if (pricesAndDiscounts.Count == 0)
                    {
                        break;
                    }

                    foreach (PriceAndDiscount priceAndDiscount in pricesAndDiscounts)
                    {
                        articuleToPriceAndDiscount[priceAndDiscount.Articule] = priceAndDiscount;
                    }

                    offset += limit;
                }
            }

            return articuleToPriceAndDiscount;
        }

        public async Task<Dictionary<int, FullInfo>> GetFullCardsInfo(DateTime todayDate)
        {
            Dictionary<int, FullInfo> articuleToFullInfo = [];
            int limit = 1000;
            int offset = 0;

            while (true)
            {
                var requestBody = new
                {
                    currentPeriod = new
                    {
                        start = $"{todayDate:yyyy-MM-dd}",
                        end = $"{todayDate:yyyy-MM-dd}"
                    },
                    orderBy = new
                    {
                        field = "addToCart",
                        mode = "desc"
                    },
                    positionCluster = "all",
                    limit,
                    offset
                };

                string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(FullCardsInfoUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);
                    JsonElement data = document.RootElement.GetProperty("data");
                    JsonElement groups = data.GetProperty("groups");
                    JsonElement items = groups.GetProperty("items");

                    List<FullInfo> fullInfos = JsonConvert.DeserializeObject<List<FullInfo>>(items.ToString())!;

                    if (fullInfos.Count == 0)
                    {
                        break;
                    }

                    foreach (FullInfo fullInfo in fullInfos)
                    {
                        articuleToFullInfo[fullInfo.Articule] = fullInfo;
                    }

                    offset += limit;
                }
                else
                {
                    Console.WriteLine("Response error");
                    break;
                }

                await Task.Delay(20000);
            }

            return articuleToFullInfo;
        }

        public async Task<Dictionary<int, CompanyStat>> GetCompaniesStat(List<int> companyIds, DateTime todayDate)
        {
            Dictionary<int, CompanyStat> articuleToCompaniesStat = [];

            try
            {
                if (companyIds.Count == 0)
                {
                    return articuleToCompaniesStat;
                }

                string urlResponse = CompaniesStatURL;

                foreach (int id in companyIds)
                {
                    if (companyIds.Count == 1)
                    {
                        urlResponse += $"?ids={id}";
                    }
                    else if (companyIds.IndexOf(id) == companyIds.Count - 1)
                    {
                        urlResponse += $"{id}";
                    }
                    else if (companyIds.IndexOf(id) == 0)
                    {
                        urlResponse += $"?ids={id},";
                    }
                    else
                    {
                        urlResponse += $"{id},";
                    }
                }

                urlResponse += $"&beginDate={todayDate:yyyy-MM-dd}&endDate={todayDate:yyyy-MM-dd}";

                HttpResponseMessage response = await _client.GetAsync(urlResponse);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(result);

                    foreach (JsonElement company in document.RootElement.EnumerateArray())
                    {
                        int advertId = company.GetProperty("advertId").GetInt32();

                        articuleToCompaniesStat[advertId] = new(advertId,
                            company.GetProperty("views").GetInt32(),
                            company.GetProperty("clicks").GetInt32(),
                            company.GetProperty("ctr").GetDouble(),
                            company.GetProperty("sum").GetDouble());
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка запроса companies stat");
                }

                return articuleToCompaniesStat;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Ошибка получения marketing stat - " + exception);
                return articuleToCompaniesStat;
            }
        }

        public async Task<List<int>> GetCompaniesByStatus(List<CompanyStatus> companiesStatus)
        {
            List<int> companiesId = [];
            HttpResponseMessage response = await _client.GetAsync(CompaniesURL);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                JsonDocument document = JsonDocument.Parse(result);

                foreach (JsonElement advert in document.RootElement.GetProperty("adverts").EnumerateArray())
                {
                    int statusValue = advert.GetProperty("status").GetInt32();

                    if (Enum.IsDefined(typeof(CompanyStatus), statusValue))
                    {
                        CompanyStatus status = (CompanyStatus)statusValue;

                        if (companiesStatus.Contains(status))
                            continue;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка получения enum status");
                        return companiesId;
                    }

                    foreach (JsonElement company in advert.GetProperty("advert_list").EnumerateArray())
                    {
                        companiesId.Add(company.GetProperty("advertId").GetInt32());
                    }
                }
            }

            return companiesId;
        }

        public async Task<Dictionary<string, Logistic>> GetLogistic(DateTime todayDate)
        {
            Dictionary<string, Logistic> wrehouseNameToLogistic = [];

            HttpResponseMessage response = await _client.GetAsync($"{LogisticUrl}?date={todayDate:yyyy-MM-dd}");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                JsonDocument document = JsonDocument.Parse(result);

                List<Logistic> logistics = JsonConvert.DeserializeObject<List<Logistic>>(document.RootElement.GetProperty("response").GetProperty("data").GetProperty("warehouseList").ToString())!;

                foreach (Logistic logistic in logistics)
                {
                    wrehouseNameToLogistic[logistic.WarehouseName] = logistic;
                }
            }

            return wrehouseNameToLogistic;
        }
    }
}