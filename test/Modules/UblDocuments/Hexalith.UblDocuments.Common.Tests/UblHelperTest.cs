namespace Hexalith.UblDocuments.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Hexalith.Application.UblDocument.Tests.Fixtures;
    using Hexalith.UblDocuments;
    using Hexalith.UblDocuments.Types;
    using Hexalith.UblDocuments.Types.Aggregates;

    using FluentAssertions;

    using Xunit;

    public class UblHelperTest
    {
        [Fact]
        public void Can_read_Invoice2_trivial_file()
        {
            File.Exists(UblTextDocument.Invoice2TrivialFile).Should().BeTrue();
            var doc = File.ReadAllBytes(UblTextDocument.Invoice2TrivialFile);
            doc.Length.Should().BeGreaterThan(100);
        }

        [Fact]
        public void IsUblInvoiceDocument_is_true()
        {
            var doc = UblTextDocument.GetInvoice2_1TrivialExampleString();
            doc.IsUblInvoiceDocument().Should().BeTrue();
        }

        [Fact]
        public void RemoveNameSpaces()
        {
            var xDocumentOriginal = XDocument.Load(UblTextDocument.Invoice2TrivialFile, LoadOptions.None);
            xDocumentOriginal.Should().NotBeNull();
            xDocumentOriginal.Root.Should().NotBeNull();
            xDocumentOriginal
                .Root
                .Attributes()
                .Where(attr => attr.IsNamespaceDeclaration)
                .ToList()
                .Should().NotBeEmpty();

            var xDocumentNew = XDocument.Parse(xDocumentOriginal.Root.RemoveAllNamespaces().ToString(), LoadOptions.None);
            xDocumentNew.Should().NotBeNull();
            xDocumentNew.Root.Should().NotBeNull();
            xDocumentNew
                .Root
                .Attributes()
                .Where(attr => attr.IsNamespaceDeclaration)
                .ToList()
                .Should().BeEmpty();
        }

        [Fact]
        public void UblInvoice_2_1_example_check_all_values()
        {
            using FileStream fs = new(UblTextDocument.Invoice2File, FileMode.Open);
            XmlSerializer xs = new(typeof(Invoice));

            #region Invoice

            var invoice = (Invoice)xs.Deserialize(fs);
            invoice.Should().NotBeNull();
            invoice.UBLVersionID.Should().Be("2.1");
            invoice.ID.Should().Be("TOSL108");
            var issueDate = invoice.IssueDateTime;
            issueDate.Year.Should().Be(2009);
            issueDate.Month.Should().Be(12);
            issueDate.Day.Should().Be(15);
            invoice.InvoiceTypeCode.Should().Be("380");
            invoice.Note.Should().Be("Ordered in our booth at the convention.");
            var taxDate = invoice.TaxPointDateTime;
            taxDate.Value.Year.Should().Be(2009);
            taxDate.Value.Month.Should().Be(11);
            taxDate.Value.Day.Should().Be(30);
            invoice.DocumentCurrencyCode.Should().Be("EUR");
            invoice.AccountingCost.Should().Be("Project cost code 123");
            invoice.InvoicePeriod.Should().NotBeNull();
            invoice.InvoicePeriod.EndDate.Should().NotBeNull();
            invoice.InvoicePeriod.StartDate.Should().NotBeNull();
            var startDate = invoice.InvoicePeriod.StartDateTime;
            startDate.Value.Year.Should().Be(2009);
            startDate.Value.Month.Should().Be(11);
            startDate.Value.Day.Should().Be(1);
            var endDate = invoice.InvoicePeriod.EndDateTime;
            endDate.Value.Year.Should().Be(2009);
            endDate.Value.Month.Should().Be(11);
            endDate.Value.Day.Should().Be(30);
            invoice.OrderReference.ID.Should().Be("123");
            invoice.ContractDocumentReference.Should().HaveCount(1);
            invoice.ContractDocumentReference[0].ID.Should().Be("Contract321");
            invoice.ContractDocumentReference[0].DocumentType.Should().Be("Framework agreement");
            invoice.AdditionalDocumentReference.Should().HaveCount(2);
            var additionalDocument1 = invoice.AdditionalDocumentReference[0];
            additionalDocument1.ID.Should().Be("Doc1");
            additionalDocument1.DocumentType.Should().Be("Timesheet");
            additionalDocument1.Attachment.ExternalReference.URI.Should().Be("http://www.suppliersite.eu/sheet001.html");
            var additionalDocument2 = invoice.AdditionalDocumentReference[1];
            additionalDocument2.ID.Should().Be("Doc2");
            additionalDocument2.DocumentType.Should().Be("Drawing");
            additionalDocument2.Attachment.EmbeddedDocumentBinaryObject.Should().Be("UjBsR09EbGhjZ0dTQUxNQUFBUUNBRU1tQ1p0dU1GUXhEUzhi");
            var supplier = invoice.AccountingSupplierParty;
            supplier.Should().NotBeNull();
            supplier.Party.Should().NotBeNull();
            supplier.Party.PartyName.Should().NotBeNull();
            supplier.Party.EndpointID.Should().Be("1234567890123");
            supplier.Party.PartyIdentification.Should().HaveCount(1);
            supplier.Party.PartyIdentification[0].ID.Should().Be("Supp123");
            supplier.Party.PartyName.Name.Should().Be("Salescompany ltd.");
            supplier.Party.PostalAddress.ID.Should().Be("1231412341324");
            supplier.Party.PostalAddress.Postbox.Should().Be("5467");
            supplier.Party.PostalAddress.StreetName.Should().Be("Main street");
            supplier.Party.PostalAddress.AdditionalStreetName.Should().Be("Suite 123");
            supplier.Party.PostalAddress.BuildingNumber.Should().Be("1");
            supplier.Party.PostalAddress.Department.Should().Be("Revenue department");
            supplier.Party.PostalAddress.CityName.Should().Be("Big city");
            supplier.Party.PostalAddress.PostalZone.Should().Be("54321");
            supplier.Party.PostalAddress.CountrySubentityCode.Should().Be("RegionA");
            supplier.Party.PostalAddress.Country.IdentificationCode.Should().Be("DK");
            supplier.Party.PartyTaxScheme.Should().HaveCount(1);
            supplier.Party.PartyTaxScheme[0].CompanyID.Should().Be("DK12345");
            supplier.Party.PartyTaxScheme[0].TaxScheme.ID.Should().Be("VAT");
            supplier.Party.PartyLegalEntity.Should().HaveCount(1);
            supplier.Party.PartyLegalEntity[0].RegistrationName.Should().Be("The Sellercompany Incorporated");
            supplier.Party.PartyLegalEntity[0].CompanyID.Should().Be("5402697509");
            supplier.Party.PartyLegalEntity[0].RegistrationAddress.CityName.Should().Be("Big city");
            supplier.Party.PartyLegalEntity[0].RegistrationAddress.CountrySubentity.Should().Be("RegionA");
            supplier.Party.PartyLegalEntity[0].RegistrationAddress.Country.IdentificationCode.Should().Be("DK");
            supplier.Party.Contact.Should().HaveCount(1);
            supplier.Party.Contact[0].Telephone.Should().Be("4621230");
            supplier.Party.Contact[0].Telefax.Should().Be("4621231");
            supplier.Party.Contact[0].ElectronicMail.Should().Be("antonio@salescompany.dk");
            supplier.Party.Person.Should().HaveCount(1);
            supplier.Party.Person[0].FirstName.Should().Be("Antonio");
            supplier.Party.Person[0].FamilyName.Should().Be("M");
            supplier.Party.Person[0].MiddleName.Should().Be("Salemacher");
            supplier.Party.Person[0].JobTitle.Should().Be("Sales manager");

            var customer = invoice.AccountingCustomerParty;
            customer.Should().NotBeNull();
            customer.Party.Should().NotBeNull();
            customer.Party.PartyName.Should().NotBeNull();
            customer.Party.EndpointID.Should().Be("1234567987654");
            customer.Party.PartyIdentification.Should().HaveCount(1);
            customer.Party.PartyIdentification[0].ID.Should().Be("345KS5324");
            customer.Party.PartyName.Name.Should().Be("Buyercompany ltd");
            customer.Party.PostalAddress.ID.Should().Be("1238764941386");
            customer.Party.PostalAddress.Postbox.Should().Be("123");
            customer.Party.PostalAddress.StreetName.Should().Be("Anystreet");
            customer.Party.PostalAddress.AdditionalStreetName.Should().Be("Back door");
            customer.Party.PostalAddress.BuildingNumber.Should().Be("8");
            customer.Party.PostalAddress.Department.Should().Be("Accounting department");
            customer.Party.PostalAddress.CityName.Should().Be("Anytown");
            customer.Party.PostalAddress.PostalZone.Should().Be("101");
            customer.Party.PostalAddress.CountrySubentity.Should().Be("RegionB");
            customer.Party.PostalAddress.Country.IdentificationCode.Should().Be("BE");
            customer.Party.PartyTaxScheme.Should().HaveCount(1);
            customer.Party.PartyTaxScheme[0].CompanyID.Should().Be("BE54321");
            customer.Party.PartyTaxScheme[0].TaxScheme.ID.Should().Be("VAT");
            customer.Party.PartyLegalEntity.Should().HaveCount(1);
            customer.Party.PartyLegalEntity[0].RegistrationName.Should().Be("The buyercompany inc.");
            customer.Party.PartyLegalEntity[0].CompanyID.Should().Be("5645342123");
            customer.Party.PartyLegalEntity[0].RegistrationAddress.CityName.Should().Be("Mainplace");
            customer.Party.PartyLegalEntity[0].RegistrationAddress.CountrySubentity.Should().Be("RegionB");
            customer.Party.PartyLegalEntity[0].RegistrationAddress.Country.IdentificationCode.Should().Be("BE");
            customer.Party.Contact.Should().HaveCount(1);
            customer.Party.Contact[0].Telephone.Should().Be("5121230");
            customer.Party.Contact[0].Telefax.Should().Be("5121231");
            customer.Party.Contact[0].ElectronicMail.Should().Be("john@buyercompany.eu");
            customer.Party.Person.Should().HaveCount(1);
            customer.Party.Person[0].FirstName.Should().Be("John");
            customer.Party.Person[0].FamilyName.Should().Be("X");
            customer.Party.Person[0].MiddleName.Should().Be("Doe");
            customer.Party.Person[0].JobTitle.Should().Be("Purchasing manager");
            var payee = invoice.PayeeParty;
            payee.Should().NotBeNull();
            payee.PartyIdentification.Should().HaveCount(1);
            payee.PartyIdentification[0].ID.Should().Be("098740918237");
            payee.PartyName.Should().NotBeNull();
            payee.PartyName.Name.Should().Be("Ebeneser Scrooge Inc.");
            payee.PartyLegalEntity.Should().HaveCount(1);
            payee.PartyLegalEntity[0].CompanyID.Should().Be("6411982340");
            var delivery = invoice.Delivery;
            delivery.Should().HaveCount(1);
            delivery[0].ActualDeliveryDate.Should().NotBeNull();
            var actualDeliveryDate = delivery[0].ActualDeliveryDateTime;
            actualDeliveryDate.Value.Year.Should().Be(2009);
            actualDeliveryDate.Value.Month.Should().Be(12);
            actualDeliveryDate.Value.Day.Should().Be(15);
            delivery[0].DeliveryLocation.Should().NotBeNull();
            var deliveryLocation = delivery[0].DeliveryLocation;
            deliveryLocation.ID.Should().Be("6754238987648");
            deliveryLocation.Address.StreetName.Should().Be("Deliverystreet");
            deliveryLocation.Address.AdditionalStreetName.Should().Be("Side door");
            deliveryLocation.Address.BuildingNumber.Should().Be("12");
            deliveryLocation.Address.CityName.Should().Be("DeliveryCity");
            deliveryLocation.Address.PostalZone.Should().Be("523427");
            deliveryLocation.Address.CountrySubentity.Should().Be("RegionC");
            deliveryLocation.Address.Country.IdentificationCode.Should().Be("BE");
            invoice.PaymentMeans.Should().HaveCount(1);
            var paymentMeans = invoice.PaymentMeans[0];
            paymentMeans.PaymentMeansCode.Should().Be("31");
            paymentMeans.PaymentDueDate.Should().NotBeNull();
            var paymentDueDate = paymentMeans.PaymentDueDateTime;
            paymentDueDate.Value.Year.Should().Be(2009);
            paymentDueDate.Value.Month.Should().Be(12);
            paymentDueDate.Value.Day.Should().Be(31);
            paymentMeans.PaymentChannelCode.Should().Be("IBAN");
            paymentMeans.PaymentID.Should().Be("Payref1");
            paymentMeans.PayeeFinancialAccount.Should().NotBeNull();
            paymentMeans.PayeeFinancialAccount.ID.Should().Be("DK1212341234123412");
            paymentMeans.PayeeFinancialAccount.FinancialInstitutionBranch.FinancialInstitution.ID.Should().Be("DKDKABCD");
            invoice.PaymentTerms.Should().HaveCount(1);
            invoice.PaymentTerms[0].Note.Should().HaveCount(1);
            invoice.PaymentTerms[0].Note[0].Should().Be("Penalty percentage 10% from due date");
            invoice.AllowanceCharge.Should().HaveCount(2);
            invoice.AllowanceCharge[0].ChargeIndicator.Should().Be(true);
            invoice.AllowanceCharge[0].AllowanceChargeReason.Should().Be("Packing cost");
            invoice.AllowanceCharge[0].Amount.Should().Be(100);
            invoice.AllowanceCharge[1].ChargeIndicator.Should().Be(false);
            invoice.AllowanceCharge[1].AllowanceChargeReason.Should().Be("Promotion discount");
            invoice.AllowanceCharge[1].Amount.Should().Be(100);
            invoice.TaxTotal.Should().NotBeNull();
            invoice.TaxTotal[0].TaxAmount.Should().Be(292.20m);
            invoice.TaxTotal[0].TaxSubtotal.Should().HaveCount(3);
            invoice.TaxTotal[0].TaxSubtotal[0].TaxableAmount.Should().Be(1460.5m);
            invoice.TaxTotal[0].TaxSubtotal[0].TaxAmount.Should().Be(292.1m);
            invoice.TaxTotal[0].TaxSubtotal[0].TaxCategory.ID.Should().Be("S");
            invoice.TaxTotal[0].TaxSubtotal[0].TaxCategory.Percent.Should().Be(20m);
            invoice.TaxTotal[0].TaxSubtotal[0].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.TaxTotal[0].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Should().Be("VAT");
            invoice.TaxTotal[0].TaxSubtotal[1].TaxableAmount.Should().Be(1m);
            invoice.TaxTotal[0].TaxSubtotal[1].TaxAmount.Should().Be(0.1m);
            invoice.TaxTotal[0].TaxSubtotal[1].TaxCategory.ID.Should().Be("AA");
            invoice.TaxTotal[0].TaxSubtotal[1].TaxCategory.Percent.Should().Be(10m);
            invoice.TaxTotal[0].TaxSubtotal[1].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.TaxTotal[0].TaxSubtotal[1].TaxCategory.TaxScheme.ID.Should().Be("VAT");
            invoice.TaxTotal[0].TaxSubtotal[2].TaxableAmount.Should().Be(-25m);
            invoice.TaxTotal[0].TaxSubtotal[2].TaxAmount.Should().Be(0m);
            invoice.TaxTotal[0].TaxSubtotal[2].TaxCategory.ID.Should().Be("E");
            invoice.TaxTotal[0].TaxSubtotal[2].TaxCategory.Percent.Should().Be(0m);
            invoice.TaxTotal[0].TaxSubtotal[2].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.TaxTotal[0].TaxSubtotal[2].TaxCategory.TaxScheme.ID.Should().Be("VAT");

            invoice.LegalMonetaryTotal.Should().NotBeNull();
            invoice.LegalMonetaryTotal.LineExtensionAmount.Should().Be(1436.5m);
            invoice.LegalMonetaryTotal.TaxExclusiveAmount.Should().Be(1436.5m);
            invoice.LegalMonetaryTotal.TaxInclusiveAmount.Should().Be(1729m);
            invoice.LegalMonetaryTotal.AllowanceTotalAmount.Should().Be(100m);
            invoice.LegalMonetaryTotal.ChargeTotalAmount.Should().Be(100m);
            invoice.LegalMonetaryTotal.PrepaidAmount.Should().Be(1000m);
            invoice.LegalMonetaryTotal.PayableRoundingAmount.Should().Be(0.30m);
            invoice.LegalMonetaryTotal.PayableAmount.Should().Be(729m);

            #endregion Invoice

            #region Lines

            var lines = invoice.InvoiceLine;
            lines.Should().NotBeNull();
            lines.Should().HaveCount(5);
            lines[0].ID.Should().Be("1");
            lines[0].Note.Should().HaveCount(1);
            lines[0].Note[0].Should().Be("Scratch on box");
            lines[0].InvoicedQuantity.Should().Be(1m);
            lines[0].LineExtensionAmount.Should().Be(1273m);
            lines[0].AccountingCost.Should().Be("BookingCode001");
            lines[0].OrderLineReference.Should().HaveCount(1);
            lines[0].OrderLineReference[0].LineID.Should().Be("1");
            lines[0].AllowanceCharge.Should().HaveCount(2);
            lines[0].AllowanceCharge[0].ChargeIndicator.Should().Be(false);
            lines[0].AllowanceCharge[0].AllowanceChargeReason.Should().Be("Damage");
            lines[0].AllowanceCharge[0].Amount.Should().Be(12);
            lines[0].AllowanceCharge[1].ChargeIndicator.Should().Be(true);
            lines[0].AllowanceCharge[1].AllowanceChargeReason.Should().Be("Testing");
            lines[0].AllowanceCharge[1].Amount.Should().Be(10);
            lines[0].TaxTotal.Should().HaveCount(1);
            lines[0].TaxTotal[0].TaxAmount.Should().Be(254.6m);

            lines[0].Item.Should().NotBeNull();
            lines[0].Item.Description.Should().Be("\n\t\t\t\tProcessor: Intel Core 2 Duo SU9400 LV (1.4GHz). RAM:\n\t\t\t\t3MB. Screen 1440x900\n\t\t\t");
            lines[0].Item.Name.Should().Be("Labtop computer");
            lines[0].Item.SellersItemIdentification.Should().NotBeNull();
            lines[0].Item.SellersItemIdentification.ID.Should().Be("JB007");
            lines[0].Item.StandardItemIdentification.Should().NotBeNull();
            lines[0].Item.StandardItemIdentification.ID.Should().Be("1234567890124");
            lines[0].Item.CommodityClassification.Should().HaveCount(2);
            lines[0].Item.CommodityClassification[0].ItemClassificationCode.Should().Be("12344321");
            lines[0].Item.CommodityClassification[1].ItemClassificationCode.Should().Be("65434568");
            lines[0].Item.ClassifiedTaxCategory.Should().HaveCount(1);
            lines[0].Item.ClassifiedTaxCategory[0].ID.Should().Be("S");
            lines[0].Item.ClassifiedTaxCategory[0].Percent.Should().Be(20);
            lines[0].Item.ClassifiedTaxCategory[0].TaxScheme.Should().NotBeNull();
            lines[0].Item.ClassifiedTaxCategory[0].TaxScheme.ID.Should().Be("VAT");
            lines[0].Item.AdditionalItemProperty.Should().HaveCount(1);
            lines[0].Item.AdditionalItemProperty[0].Name.Should().Be("Color");
            lines[0].Item.AdditionalItemProperty[0].Value.Should().Be("black");
            lines[0].Price.Should().NotBeNull();
            lines[0].Price.PriceAmount.Should().Be(1273);
            lines[0].Price.BaseQuantity.Should().Be(1);
            lines[0].Price.AllowanceCharge.Should().HaveCount(1);
            lines[0].Price.AllowanceCharge[0].ChargeIndicator.Should().Be(false);
            lines[0].Price.AllowanceCharge[0].AllowanceChargeReason.Should().Be("Contract");
            lines[0].Price.AllowanceCharge[0].MultiplierFactorNumeric.Should().Be(0.15m);
            lines[0].Price.AllowanceCharge[0].Amount.Should().Be(225m);
            lines[0].Price.AllowanceCharge[0].BaseAmount.Should().Be(1500m);
            lines[0].Price.BaseQuantity.Should().Be(1);

            #endregion Lines
        }

        [Fact]
        public void UblInvoice_2_1_from_embedded_check()
        {
            using FileStream fs = new(UblTextDocument.Invoice2FromEmbeddedFile, FileMode.Open);
            XmlSerializer xs = new(typeof(Invoice));
            var invoice = (Invoice)xs.Deserialize(fs);
            invoice.Should().NotBeNull();
            invoice.UBLVersionID.Should().Be("UBL 2.1");
            invoice.CustomizationID.Should().Be("10");
            invoice.ProfileID.Should().Be("DIAN 2.1");
            invoice.ProfileExecutionID.Should().Be("1");
            invoice.ID.Should().Be("1262");
            invoice.UUID.Should().Be("1bb8040f3c4f2ab296d771a059e10bd7e1417f01b952c2409e33ec5b719b0dab8ce5a545478f8f9d70436ae825f94158");
            var issueDate = invoice.IssueDateTime;
            issueDate.Year.Should().Be(2021);
            issueDate.Month.Should().Be(3);
            issueDate.Day.Should().Be(19);
            issueDate.Hour.Should().Be(16);
            issueDate.Minute.Should().Be(29);
            issueDate.Second.Should().Be(5);
            issueDate.Offset.Should().Be(new TimeSpan(-5, 0, 0));
            var dueDate = invoice.DueDateTime;
            dueDate.Value.Year.Should().Be(2021);
            dueDate.Value.Month.Should().Be(4);
            dueDate.Value.Day.Should().Be(3);
            invoice.InvoiceTypeCode.Should().Be("01");
            invoice.Note.Should().StartWith("Lote-Serial: 3010328-AA265M0516 y 3010328-AA265M0515");
            var taxPointDate = invoice.TaxPointDateTime;
            taxPointDate.Value.Year.Should().Be(2021);
            taxPointDate.Value.Month.Should().Be(3);
            taxPointDate.Value.Day.Should().Be(19);
            invoice.DocumentCurrencyCode.Should().Be("COP");
            invoice.LineCountNumeric.Should().Be(1);
            var supplier = invoice.AccountingSupplierParty;
            supplier.Should().NotBeNull();
            supplier.AdditionalAccountID.Should().HaveCount(1);
            supplier.AdditionalAccountID[0].Should().Be("1");
            supplier.Party.Should().NotBeNull();
            supplier.Party.PartyIdentification.Should().HaveCount(1);
            supplier.Party.PartyIdentification[0].ID.Should().Be("909812545");
            supplier.Party.PartyName.Should().NotBeNull();
            supplier.Party.PartyName.Name.Should().Be("TOTO AND CO");
            supplier.Party.PhysicalLocation.Should().NotBeNull();
            supplier.Party.PhysicalLocation.Address.Should().NotBeNull();
            supplier.Party.PhysicalLocation.Address.ID.Should().Be("11001");
            supplier.Party.PhysicalLocation.Address.CityName.Should().Be("Bogotá, D.C");
            supplier.Party.PhysicalLocation.Address.PostalZone.Should().Be("110001");
            supplier.Party.PhysicalLocation.Address.CountrySubentity.Should().Be("Bogotá");
            supplier.Party.PhysicalLocation.Address.CountrySubentityCode.Should().Be("11");
            supplier.Party.PhysicalLocation.Address.AddressLine.Should().HaveCount(1);
            supplier.Party.PhysicalLocation.Address.AddressLine[0].Line.Should().Be("2501 5th avenue");
            supplier.Party.PhysicalLocation.Address.Country.Should().NotBeNull();
            supplier.Party.PhysicalLocation.Address.Country.IdentificationCode.Should().Be("CO");
            supplier.Party.PhysicalLocation.Address.Country.Name.Should().Be("Colombia");
            supplier.Party.PartyTaxScheme.Should().HaveCount(1);
            supplier.Party.PartyTaxScheme[0].RegistrationName.Should().Be("TOTO AND CO");
            supplier.Party.PartyTaxScheme[0].CompanyID.Should().Be("909812545");
            supplier.Party.PartyTaxScheme[0].TaxLevelCode.Should().Be("O-48");
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.Should().NotBeNull();
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.ID.Should().Be("11001");
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.CountrySubentity.Should().Be("Bogotá");
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.CountrySubentityCode.Should().Be("11");
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.AddressLine.Should().HaveCount(1);
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.AddressLine[0].Line.Should().Be("2501 5th avenue");
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.Country.Should().NotBeNull();
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.Country.IdentificationCode.Should().Be("CO");
            supplier.Party.PartyTaxScheme[0].RegistrationAddress.Country.Name.Should().Be("Colombia");
            supplier.Party.PartyTaxScheme[0].TaxScheme.Should().NotBeNull();
            supplier.Party.PartyTaxScheme[0].TaxScheme.ID.Should().Be("01");
            supplier.Party.PartyTaxScheme[0].TaxScheme.Name.Should().Be("IVA");
            supplier.Party.PartyLegalEntity.Should().HaveCount(1);
            supplier.Party.PartyLegalEntity[0].RegistrationName.Should().Be("TOTO AND CO");
            supplier.Party.PartyLegalEntity[0].CompanyID.Should().Be("909812545");
            supplier.Party.Contact.Should().HaveCount(1);
            supplier.Party.Contact[0].Telephone.Should().Be("1002566854");
            supplier.Party.Contact[0].ElectronicMail.Should().Be("bernardo@europole.com");

            var customer = invoice.AccountingCustomerParty;
            customer.Should().NotBeNull();
            customer.AdditionalAccountID.Should().HaveCount(1);
            customer.AdditionalAccountID[0].Should().Be("2");
            customer.Party.Should().NotBeNull();
            customer.Party.PartyIdentification.Should().HaveCount(1);
            customer.Party.PartyIdentification[0].ID.Should().Be("35435585");
            customer.Party.PartyName.Should().NotBeNull();
            customer.Party.PartyName.Name.Should().Be("Ana Elioza Enado");
            customer.Party.PhysicalLocation.Should().NotBeNull();
            customer.Party.PhysicalLocation.Address.Should().NotBeNull();
            customer.Party.PhysicalLocation.Address.ID.Should().Be("76001");
            customer.Party.PhysicalLocation.Address.CityName.Should().Be("Cali");
            customer.Party.PhysicalLocation.Address.PostalZone.Should().Be("760001");
            customer.Party.PhysicalLocation.Address.CountrySubentity.Should().Be("Valle del Cauca");
            customer.Party.PhysicalLocation.Address.CountrySubentityCode.Should().Be("76");
            customer.Party.PhysicalLocation.Address.AddressLine.Should().HaveCount(1);
            customer.Party.PhysicalLocation.Address.AddressLine[0].Line.Should().Be("101 7th avenue");
            customer.Party.PhysicalLocation.Address.Country.Should().NotBeNull();
            customer.Party.PhysicalLocation.Address.Country.IdentificationCode.Should().Be("CO");
            customer.Party.PhysicalLocation.Address.Country.Name.Should().Be("Colombia");
            customer.Party.PartyTaxScheme.Should().HaveCount(1);
            customer.Party.PartyTaxScheme[0].RegistrationName.Should().Be("Ana Elioza Enado");
            customer.Party.PartyTaxScheme[0].CompanyID.Should().Be("35435585");
            customer.Party.PartyTaxScheme[0].TaxLevelCode.Should().Be("R-99-PN");
            customer.Party.PartyTaxScheme[0].RegistrationAddress.Should().NotBeNull();
            customer.Party.PartyTaxScheme[0].RegistrationAddress.ID.Should().Be("76001");
            customer.Party.PartyTaxScheme[0].RegistrationAddress.CityName.Should().Be("Cali");
            customer.Party.PartyTaxScheme[0].RegistrationAddress.CountrySubentity.Should().Be("Valle del Cauca");
            customer.Party.PartyTaxScheme[0].RegistrationAddress.CountrySubentityCode.Should().Be("76");
            customer.Party.PartyTaxScheme[0].RegistrationAddress.AddressLine.Should().HaveCount(1);
            customer.Party.PartyTaxScheme[0].RegistrationAddress.AddressLine[0].Line.Should().Be("101 7th avenue");
            customer.Party.PartyTaxScheme[0].RegistrationAddress.Country.Should().NotBeNull();
            customer.Party.PartyTaxScheme[0].RegistrationAddress.Country.IdentificationCode.Should().Be("CO");
            customer.Party.PartyTaxScheme[0].RegistrationAddress.Country.Name.Should().Be("Colombia");
            customer.Party.PartyTaxScheme[0].TaxScheme.Should().NotBeNull();
            customer.Party.PartyTaxScheme[0].TaxScheme.ID.Should().Be("ZY");
            customer.Party.PartyTaxScheme[0].TaxScheme.Name.Should().Be("No causa");
            customer.Party.PartyLegalEntity.Should().HaveCount(1);
            customer.Party.PartyLegalEntity[0].RegistrationName.Should().Be("Ana Elioza Enado");
            customer.Party.PartyLegalEntity[0].CompanyID.Should().Be("35435585");
            customer.Party.Contact.Should().HaveCount(1);
            customer.Party.Contact[0].Telephone.Should().Be("10655685");
            customer.Party.Contact[0].ElectronicMail.Should().Be("ventascolombia@eurolab.com");

            var taxRepresentative = invoice.TaxRepresentativeParty;
            taxRepresentative.Should().NotBeNull();
            taxRepresentative.PartyIdentification.Should().HaveCount(1);
            taxRepresentative.PartyIdentification[0].ID.Should().Be("909812545");
            taxRepresentative.PartyName.Should().NotBeNull();
            taxRepresentative.PartyName.Name.Should().Be("TOTO AND CO");

            var delivery = invoice.Delivery;
            delivery.Should().HaveCount(1);
            delivery[0].ActualDeliveryDateTime.Should().NotBeNull();
            delivery[0].ActualDeliveryDateTime.Value.Year.Should().Be(2021);
            delivery[0].ActualDeliveryDateTime.Value.Month.Should().Be(3);
            delivery[0].ActualDeliveryDateTime.Value.Day.Should().Be(19);
            delivery[0].ActualDeliveryDateTime.Value.Hour.Should().Be(16);
            delivery[0].ActualDeliveryDateTime.Value.Minute.Should().Be(29);
            delivery[0].ActualDeliveryDateTime.Value.Second.Should().Be(00);
            delivery[0].ActualDeliveryDateTime.Value.Offset.Should().Be(new TimeSpan(-5, 0, 0));

            delivery[0].DeliveryAddress.Should().NotBeNull();
            delivery[0].DeliveryAddress.ID.Should().BeEmpty();
            delivery[0].DeliveryAddress.CityName.Should().Be("Cali");
            delivery[0].DeliveryAddress.CountrySubentity.Should().BeEmpty();
            delivery[0].DeliveryAddress.CountrySubentityCode.Should().BeEmpty();
            delivery[0].DeliveryAddress.AddressLine.Should().HaveCount(1);
            delivery[0].DeliveryAddress.AddressLine[0].Line.Should().Be("TITI Corp");
            delivery[0].DeliveryAddress.Country.Should().NotBeNull();
            delivery[0].DeliveryAddress.Country.IdentificationCode.Should().BeEmpty();
            delivery[0].DeliveryAddress.Country.Name.Should().Be("Colombia");
            delivery[0].DeliveryParty.PartyName.Should().NotBeNull();
            delivery[0].DeliveryParty.PartyName.Name.Should().Be("Colombia");
            delivery[0].DeliveryParty.PhysicalLocation.Should().NotBeNull();
            delivery[0].DeliveryParty.PhysicalLocation.Address.Should().NotBeNull();
            delivery[0].DeliveryParty.PhysicalLocation.Address.ID.Should().BeEmpty();
            delivery[0].DeliveryParty.PhysicalLocation.Address.CityName.Should().Be("Cali");
            delivery[0].DeliveryParty.PhysicalLocation.Address.CountrySubentity.Should().BeEmpty();
            delivery[0].DeliveryParty.PhysicalLocation.Address.CountrySubentityCode.Should().BeEmpty();
            delivery[0].DeliveryParty.PhysicalLocation.Address.AddressLine.Should().HaveCount(1);
            delivery[0].DeliveryParty.PhysicalLocation.Address.AddressLine[0].Line.Should().Be("TITI Corp");
            delivery[0].DeliveryParty.PhysicalLocation.Address.Country.Should().NotBeNull();
            delivery[0].DeliveryParty.PhysicalLocation.Address.Country.IdentificationCode.Should().BeEmpty();
            delivery[0].DeliveryParty.PhysicalLocation.Address.Country.Name.Should().Be("Colombia");
            delivery[0].DeliveryParty.PartyTaxScheme.Should().HaveCount(1);
            delivery[0].DeliveryParty.PartyTaxScheme[0].RegistrationName.Should().BeEmpty();
            delivery[0].DeliveryParty.PartyTaxScheme[0].CompanyID.Should().BeEmpty();
            delivery[0].DeliveryParty.PartyTaxScheme[0].TaxLevelCode.Should().Be("R-99-PN");
            delivery[0].DeliveryParty.PartyTaxScheme[0].TaxScheme.Should().NotBeNull();
            delivery[0].DeliveryParty.PartyTaxScheme[0].TaxScheme.ID.Should().Be("01");
            delivery[0].DeliveryParty.PartyTaxScheme[0].TaxScheme.Name.Should().Be("IVA");
            invoice.PaymentMeans.Should().HaveCount(1);
            invoice.PaymentMeans[0].ID.Should().Be("1");
            invoice.PaymentMeans[0].PaymentMeansCode.Should().Be("31");
            invoice.PaymentMeans[0].PaymentDueDateTime.Value.Year.Should().Be(2021);
            invoice.PaymentMeans[0].PaymentDueDateTime.Value.Month.Should().Be(4);
            invoice.PaymentMeans[0].PaymentDueDateTime.Value.Day.Should().Be(3);
            invoice.PaymentTerms.Should().HaveCount(1);
            invoice.PaymentTerms[0].ReferenceEventCode.Should().Be("2");
            invoice.PaymentTerms[0].SettlementPeriod.Should().NotBeNull();
            invoice.PaymentTerms[0].SettlementPeriod.DurationMeasure.Should().Be(15);
            invoice.PrepaidPayment.Should().HaveCount(1);
            invoice.PrepaidPayment[0].ID.Should().Be("1");
            invoice.PrepaidPayment[0].PaidAmount.Should().Be(0m);
            invoice.PrepaidPayment[0].ReceivedDateTime.Value.Year.Should().Be(2021);
            invoice.PrepaidPayment[0].ReceivedDateTime.Value.Month.Should().Be(3);
            invoice.PrepaidPayment[0].ReceivedDateTime.Value.Day.Should().Be(19);
            invoice.TaxTotal.Should().HaveCount(3);
            invoice.TaxTotal[0].TaxSubtotal.Should().HaveCount(1);
            invoice.TaxTotal[0].TaxSubtotal[0].TaxAmount.Should().Be(0m);
            invoice.TaxTotal[0].TaxSubtotal[0].TaxCategory.Percent.Should().Be(0m);
            invoice.TaxTotal[0].TaxSubtotal[0].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.TaxTotal[0].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Should().Be("01");
            invoice.TaxTotal[0].TaxSubtotal[0].TaxCategory.TaxScheme.Name.Should().Be("IVA");
            invoice.TaxTotal[1].TaxSubtotal.Should().HaveCount(1);
            invoice.TaxTotal[1].TaxSubtotal[0].TaxAmount.Should().Be(0m);
            invoice.TaxTotal[1].TaxSubtotal[0].TaxCategory.Percent.Should().Be(0m);
            invoice.TaxTotal[1].TaxSubtotal[0].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.TaxTotal[1].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Should().Be("04");
            invoice.TaxTotal[1].TaxSubtotal[0].TaxCategory.TaxScheme.Name.Should().Be("Impuesto al Consumo");
            invoice.TaxTotal[2].TaxSubtotal.Should().HaveCount(1);
            invoice.TaxTotal[2].TaxSubtotal[0].TaxAmount.Should().Be(0m);
            invoice.TaxTotal[2].TaxSubtotal[0].TaxCategory.Percent.Should().Be(0m);
            invoice.TaxTotal[2].TaxSubtotal[0].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.TaxTotal[2].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Should().Be("03");
            invoice.TaxTotal[2].TaxSubtotal[0].TaxCategory.TaxScheme.Name.Should().Be("Industria Comercio Avisos");
            invoice.LegalMonetaryTotal.Should().NotBeNull();
            invoice.LegalMonetaryTotal.LineExtensionAmount.Should().Be(1m);
            invoice.LegalMonetaryTotal.TaxExclusiveAmount.Should().Be(0m);
            invoice.LegalMonetaryTotal.TaxInclusiveAmount.Should().Be(1m);
            invoice.LegalMonetaryTotal.AllowanceTotalAmount.Should().Be(0m);
            invoice.LegalMonetaryTotal.ChargeTotalAmount.Should().Be(0m);
            invoice.LegalMonetaryTotal.PrepaidAmount.Should().Be(0m);
            invoice.LegalMonetaryTotal.PayableAmount.Should().Be(1m);
            invoice.InvoiceLine.Should().HaveCount(1);
            invoice.InvoiceLine[0].ID.Should().Be("1");
            invoice.InvoiceLine[0].Note.Should().HaveCount(1);
            invoice.InvoiceLine[0].Note[0].Should().BeEmpty();
            invoice.InvoiceLine[0].InvoicedQuantity.Should().Be(2m);
            invoice.InvoiceLine[0].LineExtensionAmount.Should().Be(1);
            invoice.InvoiceLine[0].FreeOfChargeIndicator.Should().Be(false);
            invoice.InvoiceLine[0].TaxTotal.Should().HaveCount(3);
            invoice.InvoiceLine[0].TaxTotal[0].TaxSubtotal.Should().HaveCount(1);
            invoice.InvoiceLine[0].TaxTotal[0].TaxSubtotal[0].TaxAmount.Should().Be(0m);
            invoice.InvoiceLine[0].TaxTotal[0].TaxSubtotal[0].TaxCategory.Percent.Should().Be(0m);
            invoice.InvoiceLine[0].TaxTotal[0].TaxSubtotal[0].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.InvoiceLine[0].TaxTotal[0].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Should().Be("01");
            invoice.InvoiceLine[0].TaxTotal[0].TaxSubtotal[0].TaxCategory.TaxScheme.Name.Should().Be("IVA");
            invoice.InvoiceLine[0].TaxTotal[1].TaxSubtotal.Should().HaveCount(1);
            invoice.InvoiceLine[0].TaxTotal[1].TaxSubtotal[0].TaxAmount.Should().Be(0m);
            invoice.InvoiceLine[0].TaxTotal[1].TaxSubtotal[0].TaxCategory.Percent.Should().Be(0m);
            invoice.InvoiceLine[0].TaxTotal[1].TaxSubtotal[0].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.InvoiceLine[0].TaxTotal[1].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Should().Be("04");
            invoice.InvoiceLine[0].TaxTotal[1].TaxSubtotal[0].TaxCategory.TaxScheme.Name.Should().Be("Impuesto al Consumo");
            invoice.InvoiceLine[0].TaxTotal[2].TaxSubtotal.Should().HaveCount(1);
            invoice.InvoiceLine[0].TaxTotal[2].TaxSubtotal[0].TaxAmount.Should().Be(0m);
            invoice.InvoiceLine[0].TaxTotal[2].TaxSubtotal[0].TaxCategory.Percent.Should().Be(0m);
            invoice.InvoiceLine[0].TaxTotal[2].TaxSubtotal[0].TaxCategory.TaxScheme.Should().NotBeNull();
            invoice.InvoiceLine[0].TaxTotal[2].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Should().Be("03");
            invoice.InvoiceLine[0].TaxTotal[2].TaxSubtotal[0].TaxCategory.TaxScheme.Name.Should().Be("Industria Comercio Avisos");
            invoice.InvoiceLine[0].Item.Should().NotBeNull();
            invoice.InvoiceLine[0].Item.Description.Should().Be("Silicone sphere. Vol.: 500 CC. Relleno 100%");
            invoice.InvoiceLine[0].Item.SellersItemIdentification.Should().NotBeNull();
            invoice.InvoiceLine[0].Item.SellersItemIdentification.ID.Should().Be("897525");
            invoice.InvoiceLine[0].Price.Should().NotBeNull();
            invoice.InvoiceLine[0].Price.PriceAmount.Should().Be(0.5m);
            invoice.InvoiceLine[0].Price.BaseQuantity.Should().Be(2m);
        }

        [Fact]
        public void UblInvoice_Embedded_2_1_example_check()
        {
            var xml = XDocument.Load(UblTextDocument.Invoice2EmbeddedFile);
            xml.Root.Should().NotBeNull();
            xml.Root.Name.Should().NotBeNull();
            xml.Root.Name.LocalName.Should().Be(nameof(AttachedDocument));
            xml.Root.Name.NamespaceName.Should().Be(UblNamespaces.AttachedDocument2);
            XmlSerializer attachedDocumentSerializer = new(typeof(AttachedDocument));
            var attachmedDocument = (AttachedDocument)attachedDocumentSerializer.Deserialize(xml.CreateReader());
            attachmedDocument.Should().NotBeNull();
            attachmedDocument.Attachment.Should().NotBeNull();
            attachmedDocument.Attachment.ExternalReference.Should().NotBeNull();
            attachmedDocument.Attachment.ExternalReference.Description.Should().NotBeNull();
            attachmedDocument.Attachment.ExternalReference.Description.Should().Contain(UblNamespaces.Invoice2);
            var invoiceXml = XDocument.Parse(attachmedDocument.Attachment.ExternalReference.Description);
            invoiceXml.Root.Should().NotBeNull();
            invoiceXml.Root.Name.Should().NotBeNull();
            invoiceXml.Root.Name.LocalName.Should().Be(nameof(Invoice));
            invoiceXml.Root.HasElements.Should().Be(true);
            var ids = invoiceXml.Root.Elements()
                .Where(p =>
                    p.Name.LocalName == nameof(Invoice.ID) &&
                    p.Name.NamespaceName == UblNamespaces.CommonBasicComponents2 &&
                    p.Parent.Name.LocalName == nameof(Invoice)
                )
                .ToArray();
            ids.Should().HaveCount(1);
            ids[0].Should().NotBeNull();
            ids[0].Name.LocalName.Should().Be(nameof(Invoice.ID));
            ids[0].Name.NamespaceName.Should().Be(UblNamespaces.CommonBasicComponents2);
            ids[0].Value.Should().Be("1262");
            using MemoryStream stream = new();
            using XmlWriter writer = XmlWriter.Create(stream);
            invoiceXml.WriteTo(writer);
            writer.Flush();
            stream.Position = 0;
            XmlSerializer xs = new(typeof(Invoice));
            var invoice = (Invoice)xs.Deserialize(stream);
            invoice.Should().NotBeNull();
            invoice.ID.Should().Be("1262");
        }

        [Fact]
        public void UblInvoice_Trivial_2_1_example_check_all_values()
        {
            using FileStream fs = new(UblTextDocument.Invoice2TrivialFile, FileMode.Open);
            XmlSerializer xs = new(typeof(Invoice));
            var invoice = (Invoice)xs.Deserialize(fs);
            invoice.Should().NotBeNull();
            invoice.ID.Should().Be("123");

            var issueDate = invoice.IssueDateTime;
            issueDate.Year.Should().Be(2011);
            issueDate.Month.Should().Be(9);
            issueDate.Day.Should().Be(22);
            invoice.InvoicePeriod.Should().NotBeNull();
            invoice.InvoicePeriod.EndDate.Should().NotBeNull();
            invoice.InvoicePeriod.StartDate.Should().NotBeNull();
            var startDate = invoice.InvoicePeriod.StartDateTime;
            startDate.Value.Year.Should().Be(2011);
            startDate.Value.Month.Should().Be(8);
            startDate.Value.Day.Should().Be(1);
            var endDate = invoice.InvoicePeriod.EndDateTime;
            endDate.Value.Year.Should().Be(2011);
            endDate.Value.Month.Should().Be(8);
            endDate.Value.Day.Should().Be(31);
            var supplier = invoice.AccountingSupplierParty;
            supplier.Should().NotBeNull();
            supplier.Party.Should().NotBeNull();
            supplier.Party.PartyName.Should().NotBeNull();
            supplier.Party.PartyName.Name.Should().Be("Custom Cotter Pins");
            var customer = invoice.AccountingCustomerParty;
            customer.Should().NotBeNull();
            customer.Party.Should().NotBeNull();
            customer.Party.PartyName.Should().NotBeNull();
            customer.Party.PartyName.Name.Should().Be("North American Veeblefetzer");
            invoice.LegalMonetaryTotal.Should().NotBeNull();
            invoice.LegalMonetaryTotal.PayableAmount.Should().Be(100.25m);
            var lines = invoice.InvoiceLine;
            lines.Should().NotBeNull();
            lines.Should().HaveCount(1);
            lines[0].ID.Should().Be("1");
            lines[0].LineExtensionAmount.Should().Be(101.36m);
            lines[0].Item.Should().NotBeNull();
            lines[0].Item.Description.Should().Be("Cotter pin, MIL-SPEC");
        }
    }
}