//zy


using Abp.Application.Services;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application
{
    public interface IConnect_Device_With_PC_Function_Data_Application : IApplicationService
    {
        //插入,一般是批量插入
        int InsertConnect_Device_With_PC_Function_Data(AddConnect_Device_With_PC_Function_DataInput addPlcSimens_With_PC_Function_DataInput);

        //查询,按照 station的序号，要排序，现根据类型排序，保存的数据放在最后，再根据数据库中数据序号，index
        List<QuerryConnect_Device_With_PC_Function_DataOutput> QuerryConnect_Device_With_PC_Function_DataByStationNumber(int stationNumber);

        //查询所有
        List<QuerryConnect_Device_With_PC_Function_DataOutput> QuerryConnect_Device_With_PC_Function_DatasAll();


        //修改
        int ModifyConnect_Device_With_PC_Function_DataById(ModifyConnect_Device_With_PC_Function_DataByIdInput modifyPlcSimens_With_PC_Function_DataByIdInput);


        //删除
        int DeleteConnect_Device_With_PC_Function_DataById(DeleteConnect_Device_With_PC_Function_DataByIdInput deletePlcSimens_With_PC_Function_DataByIdInput);


        //清空所有
        bool ClearAll();

    }
}
