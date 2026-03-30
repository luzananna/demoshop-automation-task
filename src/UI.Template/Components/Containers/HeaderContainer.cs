using OpenQA.Selenium;
using UI.Template.Components.Basic;
using UI.Template.Framework.Extensions;
using UI.Template.Pages;

namespace UI.Template.Components.Containers;

public class HeaderContainer : BaseComponent
{
    private readonly Simple OpenBasketButton = new(By.XPath("//*[contains(@class,'cart-widget')]"));
    private readonly Simple BasketCount = new(By.XPath("//*[contains(@class,'cart-count')]"));
    private readonly Button CheckoutButton = new(By.XPath("//button[contains(.,'Checkout')]"));
    private readonly Button ClearBasketButton = new(By.XPath("//button[contains(@class,'clear-cart')]"));
    private readonly Button CloseBasketButton = new(By.XPath("//button[contains(@class,'close-cart')]"));
    private readonly Simple Title = new(By.XPath("//h1[@class='shop-title']"));

    public void OpenBasketContainer()
    {
        if (CheckoutButton.IsNotDisplayed())
        {
            OpenBasketButton.Click();
            CheckoutButton.WaitForDisplayed();
        }
    }

    public int GetBasketCount()
    {
        if (BasketCount.IsNotDisplayed())
        {
            return 0;
        }

        return int.TryParse(BasketCount.GetText(), out int count)
            ? count
            : throw new InvalidOperationException("Failed to parse basket count.");
    }

    public void CloseBasketContainer()
    {
        if (CloseBasketButton.IsDisplayed())
        {
            CloseBasketButton.Click();
            CheckoutButton.WaitForNotDisplayed();
        }
    }

    public bool GetNthProduct(int n, out string productName, out string productDetail)
    {
        productName = string.Empty;
        productDetail = string.Empty;

        string nthProductXPathSelector = $"(//*[@class='cart-item-details'])[{n}]";

        Simple nthProduct = new(By.XPath(nthProductXPathSelector));
        Simple nthProductName = new(By.XPath($"{nthProductXPathSelector}/h3"));
        Simple nthProductDetail = new(By.XPath($"{nthProductXPathSelector}/p"));

        if (nthProduct.IsDisplayed())
        {
            productName = nthProductName.GetText();
            productDetail = nthProductDetail.GetText();
            return true;
        }

        return false;
    }

    public AdminPage OpenAdminPage()
    {
        WebDriver.WaitForUrlChanged(() => Title.Click());
        return new AdminPage();
    }
}