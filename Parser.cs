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

    public static void OutputItemList(string fileName = "item.csv"){
        using(StreamWriter sw = new StreamWriter(fileName)){
            foreach(var item in itemList)
                sw.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                    item.itemNo,
                    item.itemName,
                    item.itemRank,
                    item.itemSpec,
                    item.itemURL));
            
            sw.Flush();
            sw.Close();
        }
    }
}