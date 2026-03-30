using UI.Template.Pages;

namespace UI.Template.Tests;

[TestFixture]
public class TC3AddProductTest : BaseTest
{
    private const string ProductName = "Camera M25";
    private const string ProductCategory = "Cameras";
    private const decimal ProductPrice = 50.5m;
    private const int ProductStock = 5;
    private const string ProductImage = "Camera 2";
    private const string ProductDescription = "Camera";

    [Test]
    public void AddAndVerifyNewProductTest()
    {
        // STEP 1: Open admin
        AdminPage adminPage = new AdminPage();
        adminPage.Open();

        // STEP 2: Add new product
        var addProductContainer = adminPage.OpenAddNewProductContainer();

        Assert.Multiple(() =>
        {
            Assert.That(addProductContainer.SetName(ProductName), Is.True, "Product name was not set correctly.");
            Assert.That(addProductContainer.SelectCategory(ProductCategory), Is.True, "Product category was not selected correctly.");
            Assert.That(addProductContainer.SetPrice(ProductPrice), Is.True, "Product price was not set correctly.");
            Assert.That(addProductContainer.SetStock(ProductStock), Is.True, "Product stock was not set correctly.");
            Assert.That(addProductContainer.SelectImage(ProductImage), Is.True, "Product image was not selected correctly.");
            Assert.That(addProductContainer.SetDescription(ProductDescription), Is.True, "Product description was not set correctly.");
        });

        addProductContainer.SaveChanges();

        // Optional verification in admin
        Assert.That(adminPage.TryGetProductCardByName(ProductName, out _), Is.True, "New product was not found in admin grid.");

        // STEP 3: Go to e-shop
        HomePage homePage = adminPage.GoToEshopHome();
        homePage.Open();

        // STEP 4: Open product from Cameras category
        ProductDetailPage productDetail = homePage.OpenProductByNameFromCategory(ProductCategory, ProductName);

        // STEP 5: Verify values on product detail
        Assert.Multiple(() =>
        {
            Assert.That(productDetail.ProductInfoForm.GetName(), Is.EqualTo(ProductName), "Product name does not match.");
            Assert.That(productDetail.ProductInfoForm.GetPrice(), Is.EqualTo(ProductPrice), "Product price does not match.");
            Assert.That(productDetail.ProductInfoForm.GetStockStatus(), Is.EqualTo(ProductStock), "Product stock does not match.");
        });
    }
}