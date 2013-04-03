namespace WealthLab.International.CustomersWebService
{
    using System;
    using System.CodeDom.Compiler;
    using System.Xml.Serialization;

    [Serializable, XmlType(Namespace="http://www.wealth-lab.com/"), GeneratedCode("System.Xml", "4.0.30319.233")]
    public enum AuthEducationalResult
    {
        None,
        Successfully,
        UniversityNotFound,
        KeyNotFound,
        KeyExpired,
        ProductNotEducational,
        ComputerClockDiffers,
        KeyAlreadyActivated
    }
}

