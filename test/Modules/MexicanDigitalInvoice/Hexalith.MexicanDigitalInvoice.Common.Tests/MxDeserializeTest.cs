namespace Hexalith.MexicanDigitalInvoices.Common.Tests
{
    using System.IO;
    using System.Xml.Serialization;

    using Hexalith.Application.MexicanDigitalInvoice.Tests.Fixtures;
    using Hexalith.MexicanDigitalInvoice.Aggregates;

    using FluentAssertions;

    using Xunit;

    public class MxDeserializeTest
    {
        [Fact]
        public void UblInvoice_2_1_from_embedded_check()
        {
            using FileStream fs = new(MxTextDocument.MexicanInvoiceFile, FileMode.Open);
            XmlSerializer xs = new(typeof(Voucher));
            var voucher = (Voucher)xs.Deserialize(fs);
            voucher.Should().NotBeNull();
            voucher.InvoiceId.Should().Be("2929");
            voucher.DocumentDateTime.Year.Should().Be(2021);
            voucher.DocumentDateTime.Month.Should().Be(3);
            voucher.DocumentDateTime.Day.Should().Be(9);
            voucher.DocumentDateTime.Hour.Should().Be(11);
            voucher.DocumentDateTime.Minute.Should().Be(30);
            voucher.DocumentDateTime.Second.Should().Be(12);
            voucher.DocumentDateTime.Offset.Hours.Should().Be(6);
            voucher.Issuer.Should().NotBeNull();
            voucher.Issuer.Code.Should().Be("DIOR02525DXA");
            voucher.Issuer.Name.Should().Be("GLOBAL CONSOLIDATED DIOR MEXICO S DE RL DE CV");
            voucher.Issuer.TaxCode.Should().Be("601");
            voucher.Receiver.Should().NotBeNull();
            voucher.Receiver.Code.Should().Be("CUST00422000");
            voucher.Receiver.Name.Should().Be("GRUPO VENTA INTERNACIONAL, S.A. DE C.V.");
            voucher.Receiver.UsageCode.Should().Be("G01");
            voucher.TaxItems.Should().NotBeNull();
            voucher.TaxItems.Items.Should().HaveCount(6);
            voucher.TaxItems.Items[0].ItemCode.Should().Be("42295509");
            voucher.TaxItems.Items[1].ItemCode.Should().Be("42295509");
            voucher.TaxItems.Items[2].ItemCode.Should().Be("42295509");
            voucher.TaxItems.Items[3].ItemCode.Should().Be("42295509");
            voucher.TaxItems.Items[4].ItemCode.Should().Be("42295509");
            voucher.TaxItems.Items[5].ItemCode.Should().Be("42295509");
            voucher.TaxItems.Items[0].Quantity.Should().Be(24);
            voucher.TaxItems.Items[1].Quantity.Should().Be(2);
            voucher.TaxItems.Items[2].Quantity.Should().Be(2);
            voucher.TaxItems.Items[3].Quantity.Should().Be(4);
            voucher.TaxItems.Items[4].Quantity.Should().Be(24);
            voucher.TaxItems.Items[5].Quantity.Should().Be(5);
            voucher.Tax.Should().NotBeNull();
            voucher.Tax.Total.Should().Be(1351.66m);
            voucher.Tax.Transactions.Should().NotBeNull();
            voucher.Tax.Transactions.Transaction.Should().HaveCount(1);
            voucher.Tax.Transactions.Transaction[0].TaxCode.Should().Be("002");
            voucher.Tax.Transactions.Transaction[0].FactorType.Should().Be("Tasa");
            voucher.Tax.Transactions.Transaction[0].Percent.Should().Be(0.16m);
            voucher.Tax.Transactions.Transaction[0].Amount.Should().Be(1351.66m);
            voucher.Complement.Should().NotBeNull();
            voucher.Complement.RevenueStamp.Should().NotBeNull();
            voucher.Complement.RevenueStamp.Version.Should().Be("1.1");
            voucher.Addendum.Should().NotBeNull();
            var invoice = voucher.Addendum.Invoice;
            invoice.Should().NotBeNull();
            invoice.Version.Should().Be(7);
            invoice.Identification.IssuerCountryCode.Should().Be("MX");
            invoice.Identification.DocumentType.Should().Be("FACTURA");
            invoice.Identification.IssuerCode.Should().Be("DIOR02525DXA");
            invoice.Identification.IssuerName.Should().Be("GLOBAL CONSOLIDATED DIOR MEXICO S DE RL DE CV");
            invoice.Identification.UserCode.Should().Be("DIOR02525DXAUser");
            invoice.Identification.DeliveryLocation.Should().Be("06500");
            invoice.Identification.Signature.Should().Be("3qe7qlscQY62tlnEM3JMsxA3WXSQ==");
            invoice.Processing.Should().NotBeNull();
            invoice.Processing.Dictionary.Should().NotBeNull();
            invoice.Processing.Dictionary.Name.Should().Be("email");
            invoice.Processing.Dictionary.Entry.Should().HaveCount(3);
            invoice.Processing.Dictionary.Entry[0].Key.Should().Be("from");
            invoice.Processing.Dictionary.Entry[1].Key.Should().Be("to");
            invoice.Processing.Dictionary.Entry[2].Key.Should().Be("cc");
            invoice.Processing.Dictionary.Entry[0].Value.Should().Be("ACCOUNT_OWNER");
            invoice.Processing.Dictionary.Entry[1].Value.Should().Be("luis@gvi.com.mx; lupita@gvi.com.mx;eduardoz@esta.com");
            invoice.Processing.Dictionary.Entry[2].Value.Should().Be("bernardo@gesta.com");
            invoice.Seller.Should().NotBeNull();
            invoice.Seller.TaxResidence.Should().NotBeNull();
            invoice.Seller.TaxResidence.Street.Should().Be("AVE DE LA LIBERATION");
            invoice.Seller.TaxResidence.StreetNumber.Should().Be("31");
            invoice.Seller.TaxResidence.AppartmentNumber.Should().Be("PISO 21");
            invoice.Seller.TaxResidence.CityLocation.Should().Be("CUAUTHEMOC");
            invoice.Seller.TaxResidence.City.Should().Be("CUAUTHEMOC");
            invoice.Seller.TaxResidence.CountryLocation.Should().Be("CIUDAD DE MEXICO");
            invoice.Seller.TaxResidence.Country.Should().Be("MEXICO");
            invoice.Seller.TaxResidence.ZipCode.Should().Be("06500");
            invoice.Customer.Should().NotBeNull();
            invoice.Customer.Residence.Should().NotBeNull();
            invoice.Customer.Residence.TaxResidence.Should().NotBeNull();
            invoice.Customer.Residence.TaxResidence.Street.Should().Be("CALLE ROBERTO ESTIA 256");
            invoice.Customer.Residence.TaxResidence.AppartmentNumber.Should().Be("Piso 10");
            invoice.Customer.Residence.TaxResidence.CityLocation.Should().Be("COL. PROVIDENCIA 1A 2A Y 3A SECCION");
            invoice.Customer.Residence.TaxResidence.City.Should().Be("GUADALAJARA");
            invoice.Customer.Residence.TaxResidence.CountryLocation.Should().Be("JALISCO");
            invoice.Customer.Residence.TaxResidence.Country.Should().Be("MEXICO");
            invoice.Customer.Residence.TaxResidence.ZipCode.Should().Be("44630");
            invoice.InvoiceLines.Should().HaveCount(6);
            invoice.InvoiceLines[0].Quantity.Should().Be(24m);
            invoice.InvoiceLines[0].KeyUnit.Should().Be("H87");
            invoice.InvoiceLines[0].MesureUnit.Should().Be("Unidad");
            invoice.InvoiceLines[0].ProductService.Should().Be("42295509");
            invoice.InvoiceLines[0].Description.Should().Be("281300 Sphere Gel Cohesivo Suave, Redondo, Liso, Extra Proyectado, 300 cc");
            invoice.InvoiceLines[0].Price.Should().Be(134.1m);
            invoice.InvoiceLines[0].LineAmount.Should().Be(3218.40m);
        }
    }
}