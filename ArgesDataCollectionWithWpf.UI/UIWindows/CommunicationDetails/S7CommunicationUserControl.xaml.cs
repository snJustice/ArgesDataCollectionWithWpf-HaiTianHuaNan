using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.Application.SerializeApplication;
using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.Communication.S7Communication;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel.SimensS7;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CommunicationDetails
{


    

    /// <summary>
    /// S7CommunicationUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class S7CommunicationUserControl : UserControl
    {
        private  QuerryCommunicationDetailsAndInstanceOutput _querryCommunicationDetailsAndInstanceOutput;
        private readonly ICommunicationDetailsAndInstanceApplication _iCommunicationDetailsAndInstanceApplication;

        

        public S7CommunicationUserControl(QuerryCommunicationDetailsAndInstanceOutput querryCommunicationDetailsAndInstanceOutput , ICommunicationDetailsAndInstanceApplication iCommunicationDetailsAndInstanceApplication)
        {
            InitializeComponent();
            this._querryCommunicationDetailsAndInstanceOutput = querryCommunicationDetailsAndInstanceOutput;
            this._iCommunicationDetailsAndInstanceApplication = iCommunicationDetailsAndInstanceApplication;
        }

        private void On_UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            
            

            if (this._querryCommunicationDetailsAndInstanceOutput.SerialResult!=null &&
                this._querryCommunicationDetailsAndInstanceOutput.SerialResult.Count()>0)
            {
                var plcPara = (PlcSimensConnectionPara)CustomerSerialize.DeSerializeBinaryToObject(this._querryCommunicationDetailsAndInstanceOutput.SerialResult);
                this.txt_IP.Text = plcPara.PLCIPAddress;
                this.txt_Port.Text = plcPara.PLCPort.ToString();
                this.txt_rack.Text = plcPara.Rack.ToString();
                this.txt_slot.Text = plcPara.Slot.ToString();
                this.cbx_Plc_Type.SelectedItem = plcPara.CpuType;
            }
            
        }

        

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

            //直接往数据库保存吧
            var querryresult = this._iCommunicationDetailsAndInstanceApplication.QuerryCommunicationDetailsAndInstanceByUnicode( this._querryCommunicationDetailsAndInstanceOutput.UniqueCode);
            PlcSimensConnectionPara plcSimensConnectionPara = new PlcSimensConnectionPara
            {
                PLCIPAddress = txt_IP.Text,
                CpuType  = (CpuType)Enum.Parse(typeof(CpuType),this.cbx_Plc_Type.Text , false),
                Slot = Convert.ToInt32(txt_slot.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                Rack = Convert.ToInt32(txt_rack.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                PLCPort = Convert.ToInt32(txt_Port.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                


            };
            var serial = CustomerSerialize.SerializeToBinary(plcSimensConnectionPara, typeof(PlcSimensConnectionPara));
            if (querryresult != null && querryresult.Count==0)
            {
                //插入
                

                

                //this._iCommunicationDetailsAndInstanceApplication.InsertCommunicationDetailsAndInstance(
                //    new AddCommunicationDetailsAndInstanceInput { 
                //        ConnectType = this._querryCommunicationDetailsAndInstanceOutput.ConnectType,
                //        SerialResult= serial,
                //        ID = this._querryCommunicationDetailsAndInstanceOutput.ID,
                //        UniqueCode = this._querryCommunicationDetailsAndInstanceOutput.UniqueCode,
                //    }
                //    );
            }
            else
            {
                //更新
                this._iCommunicationDetailsAndInstanceApplication.ModifyCommunicationDetailsAndInstanceById(
                    new ModifyCommunicationDetailsAndInstanceByIdInput
                    {
                        ConnectType = this._querryCommunicationDetailsAndInstanceOutput.ConnectType,
                        SerialResult = serial,
                        ID = this._querryCommunicationDetailsAndInstanceOutput.ID,
                        UniqueCode = this._querryCommunicationDetailsAndInstanceOutput.UniqueCode,
                    }
                    );

            }

        }

        private async void btn_ConnectTest_Click(object sender, RoutedEventArgs e)
        {

            PlcSimensConnectionPara plcSimensConnectionPara = new PlcSimensConnectionPara
            {
                PLCIPAddress = txt_IP.Text,
                CpuType = (CpuType)Enum.Parse(typeof(CpuType), this.cbx_Plc_Type.Text, false),
                Slot = Convert.ToInt32(txt_slot.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                Rack = Convert.ToInt32(txt_rack.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                PLCPort = Convert.ToInt32(txt_Port.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),



            };



            IDevice s7 = new ArgesSimensS7Communication( null,null, plcSimensConnectionPara);
            

            try
            {

                var re = await s7.OpenAsync().ConfigureAwait(false);
                if (((IConnected)s7).IsConnected)
                {
                    MessageBox.Show("连接成功");
                }
                else
                {
                    MessageBox.Show("连接失败");
                }
            }
            catch (Exception ex )
            {

                MessageBox.Show("连接失败");
            }
            finally
            {
                if (s7 != null && ((IConnected)s7).IsConnected)
                {
                    s7.Close();
                }
            }
        }
    }
}
