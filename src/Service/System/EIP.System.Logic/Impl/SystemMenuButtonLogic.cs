/**************************************************************
* Copyright (C) 2022 www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2022/01/12 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
*
**************************************************************/
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// ������ҵ���߼�
    /// </summary>
    public class SystemMenuButtonLogic : DapperAsyncLogic<SystemMenuButton>, ISystemMenuButtonLogic
    {
        #region ���캯��
        private readonly ISystemMenuButtonRepository _functionRepository;

        /// <summary>
        /// ģ�鰴ť
        /// </summary>
        /// <param name="functionRepository"></param>
        public SystemMenuButtonLogic(ISystemMenuButtonRepository functionRepository)
        {
            _functionRepository = functionRepository;
        }

        #endregion

        #region ����

        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemMenuButtonFindOutput>>> Find(SystemMenuButtonFindInput input)
        {
            return OperateStatus<PagedResults<SystemMenuButtonFindOutput>>.Success(await _functionRepository.Find(input));
        }

        /// <summary>
        /// ���湦������Ϣ
        /// </summary>
        /// <param name="menuButton">��������Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemMenuButton menuButton)
        {
            var menuButtonData = await FindAsync(f => f.MenuButtonId == menuButton.MenuButtonId);
            var currentUser = EipHttpContext.CurrentUser();
            if (menuButtonData == null)
            {
                menuButton.MenuButtonId = menuButton.MenuButtonId.IsEmptyGuid() ? CombUtil.NewComb() : menuButton.MenuButtonId;
                menuButton.CreateTime = DateTime.Now;
                menuButton.CreateUserId = currentUser.UserId;
                menuButton.CreateUserName = currentUser.Name;
                menuButton.UpdateTime = DateTime.Now;
                menuButton.UpdateUserId = currentUser.UserId;
                menuButton.UpdateUserName = currentUser.Name;
                return await InsertAsync(menuButton);
            }
            menuButton.Id = menuButtonData.Id;
            menuButton.CreateTime = menuButtonData.CreateTime;
            menuButton.CreateUserId = menuButtonData.CreateUserId;
            menuButton.CreateUserName = menuButtonData.CreateUserName;

            menuButton.UpdateTime = DateTime.Now;
            menuButton.UpdateUserId = currentUser.UserId;
            menuButton.UpdateUserName = currentUser.Name;
            return await UpdateAsync(menuButton);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemMenuButton>> FindById(IdInput input)
        {
            return OperateStatus<SystemMenuButton>.Success(await FindAsync(f => f.MenuButtonId == input.Id));
        }

        /// <summary>
        /// ɾ��������
        /// </summary>
        /// <param name="input">��������Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            var operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var id in input.Id.Split(','))
                    {
                        var gId = Guid.Parse(id);
                        var privilegeAccess = EnumPrivilegeAccess.ģ�鰴ť.ToShort();
                        var deleteResult = await fixture.Db.SystemPermission.DeleteAsync(f => f.PrivilegeAccess == privilegeAccess && f.PrivilegeAccessValue == gId, trans);
                        deleteResult = await fixture.Db.SystemMenuButton.DeleteAsync(d => d.MenuButtonId == gId, trans);
                    }
                    operateStatus.Msg = Chs.Successful;
                    operateStatus.Code = ResultCode.Success;
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    operateStatus.Msg = ex.Message;
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.MenuButtonId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// �Ƿ���ʾ���б�
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsShowTable(IdInput input)
        {
            var data = await FindAsync(f => f.MenuButtonId == input.Id);
            data.IsShowTable = !data.IsShowTable;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuButton"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperateStatus> SaveAll(SystemMenuButtonSaveAllInput input)
        {
            List<SystemMenuButton> menuButtons = input.MenuButton.JsonStringToObject<List<SystemMenuButton>>();
            var buttons = await FindAllAsync(f => f.MenuId == input.MenuId);
            var deletes = new List<Guid>();
            foreach (var button in buttons)
            {
                var have = menuButtons.Any(a => a.MenuButtonId == button.MenuButtonId);
                if (!have)
                {
                    deletes.Add(button.MenuButtonId);
                }
            }

            if (deletes.Count > 0)
            {
                await Delete(new IdInput<string>
                {
                    Id = deletes.ExpandAndToString()
                });
            }

            for (int i = 0; i < menuButtons.Count; i++)
            {
                var button = menuButtons[i];
                button.MenuId = input.MenuId;
                button.OrderNo = i;
                if (button.MenuButtonId.IsEmptyGuid())
                {
                    var currentUser = EipHttpContext.CurrentUser();
                    button.MenuButtonId = CombUtil.NewComb();
                    button.CreateTime = DateTime.Now;
                    button.CreateUserId = currentUser.UserId;
                    button.CreateUserName = currentUser.Name;
                    button.UpdateTime = DateTime.Now;
                    button.UpdateUserId = currentUser.UserId;
                    button.UpdateUserName = currentUser.Name;
                    await InsertAsync(button);
                }
                else
                {
                    await Save(button);
                }
            }

            return OperateStatus.Success("����ɹ�");
        }
        #endregion
    }
}