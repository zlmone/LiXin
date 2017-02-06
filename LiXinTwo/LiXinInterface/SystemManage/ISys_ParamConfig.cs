using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;

namespace LiXinInterface.SystemManage
{
    public interface ISys_ParamConfig
    {

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void InsertSys_ParamConfig(Sys_ParamConfig model);

        /// <summary>
        /// 修改参数配置
        /// </summary>
        /// <param name="ConfigType">1:Email配置;2:短信配置</param>
        /// <param name="ConfigValue">Email参数:用户名;密码;接受服务器;端口号;发送邮件服务器,短信配置:帐号;用户名;密码</param>
        /// <returns></returns>
        bool UpadteSys_ParamConfig(int ConfigType, string ConfigValue, int useType = 0);


        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="configType"></param>
        /// <returns></returns>
        Sys_ParamConfig GetSys_ParamConfig(int configType);

        /// <summary>
        /// 获取所有参数配置
        /// configType/configValue
        /// 1/邮箱配置
        /// 2/短信配置
        /// 3/培训年度设定
        /// 7/培训年度设定
        /// 8/CPA培训周期设定
        /// 9/培训级别统一调整时间设定
        /// 10/各部门/分所培训负责人设定时限
        /// 11/培训需求上报时间设定
        /// 13/各级别当年度内部培训的目标学时
        /// 14/各培训形式当年度内部培训的获取学时上限
        /// 16/CPA年度考核目标学时
        /// 17/CPA培训周期考核目标学时
        /// 18/必修/选修课程学时获取上限
        /// 19/全年课程退订次数
        /// 20/全年课程补预定次数
        /// 21/排队状态更改为正常预定状态的时间点
        /// 22/请假审核时限
        /// 23/补预定审核时限
        /// 24/集中授课学时折算分布
        /// 25/视频课程课后可参加在线测试条件
        /// 26/在线测试允许答题时间
        /// 27/在线测试允许最大次数
        /// 29/课前建议、课后评估的开放时间
        /// 30/参加课后评估获取奖励学时上限
        /// 
        /// </summary>
        /// <returns></returns>
        List<Sys_ParamConfig> GetList(string where = "1=1");

        /// <summary>
        /// 更新用途
        /// </summary>
        /// <param name="type1">ConfigType</param>
        /// <param name="type2">ConfigType</param> 
        void updateUseType(int type1, int type2);

        bool UpadteSys_ParamConfigByYear(int ConfigType, string ConfigValue, int year, int useType = 0);
    }
}
