// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


var driver = new ChromeDriver();

driver.Navigate().GoToUrl("https://iosys.co.jp/items/smartphone?page=1#bc");

var itemForm = driver.FindElements(By.CssSelector("form[id]"));

string itemNo,itemName,itemRank,itemSpec,itemURL;

foreach(var item in itemForm){
    itemNo = "";
    itemName = "";
    itemRank= "";
    itemSpec="";
    itemURL= "";
    
    var gnNode = item.FindElement(By.CssSelector("input[name='gn']"));
    if(gnNode!= null){
        itemNo = gnNode.GetAttribute("value");
    }

    var nameNode = item.FindElement(By.CssSelector("input[name='name']"));
    if(nameNode!= null){
        itemName = nameNode.GetAttribute("value");
    }
    
    var rankNode = item.FindElement(By.CssSelector("input[name='rank']"));
    if(rankNode!= null){
        itemRank = rankNode.GetAttribute("value");
    }

    var specNode = item.FindElement(By.CssSelector("input[name='spec']"));
    if(specNode!= null){
        itemSpec = specNode.GetAttribute("value");
    }
    var urlNode = item.FindElement(By.CssSelector("input[name='url']"));
    if(urlNode!= null){
        itemURL = urlNode.GetAttribute("value");
    }

    Console.WriteLine("{0},{1},{2},{3},{4}",itemNo,itemName,itemRank,itemSpec,itemURL);
}



driver.Quit();


Console.WriteLine("test");