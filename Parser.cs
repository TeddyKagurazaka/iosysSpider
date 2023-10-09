using System.Drawing.Text;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using OpenQA.Selenium;

public static class iosysParser{
    public class iosysItemModel{
        public string itemNo = "";
        public string itemName = "";
        public string itemRank = "";
        public string itemSpec = "";
        public string itemURL = "";

        public iosysItemModel(string no,string name,string rank,string spec,string url){
            itemNo = no;
            itemName = name;
            itemRank = rank;
            itemSpec = spec;
            itemURL = url;
        }

        public iosysItemModel(){}
    }

    public class iosysItemModelV2 : iosysItemModel{
        public string maker = "";
        public string releaseDate = "";
        public string stock = "";
        public string whatInside = "";
        public string price = "";

        public iosysItemModelV2(string no,string name,string rank,string spec,string url,
        string mkr,string rDate,string stk,string whatIn,string prc):
            base(no,name,rank,spec,url){
                maker = mkr;
                releaseDate = rDate;
                stock = stk;
                whatInside = whatIn;
                price = prc;
            }
        
        public iosysItemModelV2(){}
    }

    //Try to get total pages.
    public static int FindLastPage(WebDriver driver){
        //The last 2 always be: (last page)|(next icon) so find that specifically
        var pageLinks = driver.FindElements(By.CssSelector("a[class='page-link']"));
        int lastPage = 1;
        foreach(var pages in pageLinks.Reverse()){
            if(int.TryParse(pages.Text,out lastPage)){
                return lastPage;
            }
        }
        return lastPage;
    }

    public static List<iosysItemModel> itemList = new List<iosysItemModel>();
    
    public static void ParseItemInfo(WebDriver driver){
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
            Console.WriteLine("{0},{1},{2}",itemNo,itemName,itemRank);

            itemList.Add(new iosysItemModel(itemNo,itemName,itemRank,itemSpec,itemURL));
        }
    }

    public static List<iosysItemModelV2> itemListV2 = new List<iosysItemModelV2>();
    
    public static void ParseItemInfoV2(WebDriver driver){
        var itemNode = driver.FindElements(By.CssSelector("li[class='col-lg-3 col-md-3 col-sm-4 col-xs-6 item']"));
        foreach(var item in itemNode){
            var newItem = new iosysItemModelV2();

            var idNode = item.FindElement(By.CssSelector("input[name='gn']"));
            if(idNode != null){
                newItem.itemNo = idNode.GetAttribute("value");
            }

            var name = item.FindElement(By.CssSelector("input[name='name']"));
            if(name != null){
                newItem.itemName = name.GetAttribute("value");
            }

            var cond = item.FindElement(By.CssSelector("input[name='rank']"));
            if(cond != null){
                newItem.itemRank = cond.GetAttribute("value");
            }
            var spec = item.FindElement(By.CssSelector("input[name='spec']"));
            if(spec!= null){
                newItem.itemSpec = spec.GetAttribute("value");
            }

            var href = item.FindElement(By.CssSelector("input[name='url']"));
            if(href != null){
                newItem.itemURL = href.GetAttribute("value");
            }

            //New Item
            var maker = item.FindElement(By.CssSelector("p[class='maker']"));
            if(maker != null){
                newItem.maker = maker.Text.Replace("メーカー：","");
            }

            var release = item.FindElement(By.CssSelector("p[class='release']"));
            if(release != null){
                newItem.releaseDate = release.Text.Replace("発売日：","");
            }

            var stock = item.FindElement(By.CssSelector("p[class='stock']"));
            if(stock != null){
                newItem.stock = stock.Text.Replace("在庫数：","");
            }

            var whatInside = item.FindElement(By.CssSelector("p[class='accessory']"));
            if(whatInside != null){
                newItem.whatInside = whatInside.Text.Replace("付属品: ","");
            }

            var price = item.FindElement(By.CssSelector("div[class='price']"));
            if(price != null){
                newItem.price = price.Text.Replace(" ","").Replace("\n","").Replace("円(税込)","");
            }
            Console.WriteLine("{0},{1},{2},{3}",newItem.itemNo,newItem.itemName,newItem.itemRank,newItem.price);
            itemListV2.Add(newItem);
        }
    }

    public static void OutputItemList(string fileName = "item.csv"){
        using(StreamWriter sw = new StreamWriter(fileName)){
            foreach(var item in itemList)
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"",
                    item.itemNo,
                    item.itemName,
                    item.itemRank,
                    item.itemSpec,
                    item.itemURL));
            
            sw.Flush();
            sw.Close();
        }
    }

    public static void OutputItemListV2(string fileName = "item2.csv"){
        using(StreamWriter sw = new StreamWriter(fileName)){
            sw.WriteLine("No,Name,Rank,Spec,Url,Maker,ReleaseDate,Stock,WhatInside,Price(Yen,Tax.Incl)");
            foreach(var item in itemListV2)
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\"",
                    item.itemNo,
                    item.itemName,
                    item.itemRank,
                    item.itemSpec,
                    item.itemURL,
                    item.maker,
                    item.releaseDate,
                    item.stock,
                    item.whatInside,
                    item.price));
            
            sw.Flush();
            sw.Close();
        }
    }
}