//zy


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.DbModels.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EnumAddressFunction
    {
        [Description("读取保存-ReadAndNeedSaveData")]
        ReadAndNeedSaveData,//需要读取的数据,要保存的数据

        [Description("OK保存-SaveOK")]
        SaveOK,//数据上传成功

        [Description("NG保存-SaveNG")]
        SaveFail,//数据上传失败

        [Description("心跳-socket")]
        Socket,//心跳信号


        [Description("触发-Trigger")]
        Trigger,//轮询plc触发信号的功能

        [Description("读取显示-ReadAndNeedUpShowOnUi")]
        ReadAndNeedUpShowOnUi,//需要读取的数据，不需要保存，只需要显示

        [Description("界面写入-UIWriteData")]
        UIWriteData,//界面上操作后，写入到plc里面去的数据

        [Description("读取保存不通过PLC-ReadAndNeedSaveDataNotFromPLC")]
        ReadAndNeedSaveDataNotFromPLC,//界面上操作后，写入到plc里面去的数据

        [Description("下发完成信息-SendModlingAndPollRodDone")]
        SendModlingAndPollRodDone,//

        [Description("写到plc机型-ModlingTypeName")]
        ModlingTypeName,//机型写入到plc里面去的数据

        [Description("写到plc数量-ProduceQuality")]
        ProduceQuality,//数量


        [Description("写到plc拉杆型号-PollRod")]
        PollRod,//拉杆型号


        [Description("写到plc当月产量信息-PollRodMonth")]
        MonthProductionOutput,//当月产量

        [Description("写到plc当日产量信息-PollRodDay")]
        DayProductionOutput,//当日产量
    }



    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type) : base(type)
        {
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (null != value)
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());

                    if (null != fi)
                    {
                        var attributes =
                            (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        return ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Description)))
                            ? attributes[0].Description
                            : value.ToString();
                    }
                }

                return string.Empty;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
