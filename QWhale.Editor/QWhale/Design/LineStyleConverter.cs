namespace QWhale.Design
{
    using QWhale.Editor;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;

    public class LineStyleConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType == typeof(InstanceDescriptor)) && typeof(EditLineStyle).IsInstanceOfType(value))
            {
                ConstructorInfo constructor = typeof(EditLineStyle).GetConstructor(new Type[0]);
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, null, false);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

