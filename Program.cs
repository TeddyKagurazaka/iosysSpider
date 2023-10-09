using System.Collections;
// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


var driver = new ChromeDriver();

var endpoints = new string[]{
    "https://iosys.co.jp/items/smartphone",
    "https://iosys.co.jp/items/tablet",
    "https://iosys.co.jp/items/audio",
    "https://iosys.co.jp/items/featurephone",
    "https://iosys.co.jp/items/mobile-router",
    "https://iosys.co.jp/items/smartphonegoods",
    "https://iosys.co.jp/items/home_varietygoods",
    "https://iosys.co.jp/items/wearable",
    "https://iosys.co.jp/items/pc"
};

foreach(var endpoint in endpoints){
    var param = endpoint.Split('/',StringSplitOptions.RemoveEmptyEntries);
    var categoty = param[param.Length - 1];
    
    iosysParser.itemList.Clear();
    driver.Navigate().GoToUrl(endpoint);
    var lastPage = iosysParser.FindLastPage(driver);
    Console.WriteLine("{0},{1}",categoty,lastPage);

    for(int i = 1;i <= lastPage;i++)
    {
        Console.WriteLine("{0} - Page:{1}/{2}",categoty,i,lastPage);
        driver.Navigate().GoToUrl(
                endpoint + "?page=" + i + "#bc"
            //string.Format("https://iosys.co.jp/items/smartphone?page={0}#bc",i)
        );
        iosysParser.ParseItemInfoV2(driver);
    }
    iosysParser.OutputItemListV2(string.Format("{0}_2.csv",categoty));
}

driver.Quit();
Console.WriteLine("over");

