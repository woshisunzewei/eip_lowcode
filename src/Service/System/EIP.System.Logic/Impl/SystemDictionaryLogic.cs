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
using EIP.System.Models.Dtos.Dictionary;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// �ֵ�ҵ���߼�ʵ��
    /// </summary>
    public class SystemDictionaryLogic : DapperAsyncLogic<SystemDictionary>, ISystemDictionaryLogic
    {
        #region ���캯��

        private readonly ISystemDictionaryRepository _dictionaryRepository;
        /// <summary>
        /// �ֵ�
        /// </summary>
        /// <param name="dictionaryRepository"></param>
        public SystemDictionaryLogic(ISystemDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        #endregion

        #region ����

        /// <summary>
        /// ��ѯ�����ֵ���Ϣ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> Tree()
        {
            var config = new MapperConfigurationExpression();
            config.CreateMap<SystemDictionary, BaseTree>()
           .ForMember(o => o.id, opt => opt.MapFrom(src => src.DictionaryId))
           .ForMember(o => o.parent, opt => opt.MapFrom(src => src.ParentId))
           .ForMember(o => o.text, opt => opt.MapFrom(src => src.Name));
            return OperateStatus<IEnumerable<BaseTree>>.Success((await FindAllAsync()).OrderBy(o => o.OrderNo).MapToList<SystemDictionary, BaseTree>(config));
        }

        /// <summary>
        ///    ���ݸ���id��ȡ�¼�(ֻ��һ��)
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDictionary>>> FindDictionaryByParentId(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemDictionary>>.Success((await FindAllAsync(f => f.ParentId == input.Id)).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        /// ���ݸ�����ѯ�¼�(�����¼�)
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDictionaryFindOutput>>> Find(SystemDictionaryFindInput input)
        {
            IList<SystemDictionaryFindOutput> outputs = new List<SystemDictionaryFindOutput>();
            var data = (await _dictionaryRepository.Find(input)).ToList();
            foreach (var item in data)
            {
                item.Number = data.Count(c => c.ParentId == item.DictionaryId);
            }
            //�������и���������ǰ��
            var parents = data.Where(w => w.Number > 0).OrderBy(o => o.OrderNo).ToList();
            var lasts = data.Where(w => w.Number == 0).OrderBy(o => o.OrderNo).ToList();
            foreach (var item in parents)
            {
                outputs.Add(item);
            }
            foreach (var item in lasts)
            {
                outputs.Add(item);
            }
            return OperateStatus<IEnumerable<SystemDictionaryFindOutput>>.Success(outputs);
        }

        /// <summary>
        /// �����ֵ���Ϣ
        /// </summary>
        /// <param name="input">�ֵ���Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemDictionary input)
        {
            OperateStatus operateStatus;
            var dictionary = await FindAsync(f => f.DictionaryId == input.DictionaryId);
            var currentUser = EipHttpContext.CurrentUser();
            if (dictionary == null)
            {
                input.CanbeDelete = true;
                input.DictionaryId = CombUtil.NewComb();

                input.CreateTime = DateTime.Now;
                input.CreateUserId = currentUser.UserId;
                input.CreateUserName = currentUser.Name;
                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;

                operateStatus = await InsertAsync(input);
            }
            else
            {
                input.Id = dictionary.Id;
                input.CanbeDelete = dictionary.CanbeDelete;
                input.CreateTime = dictionary.CreateTime;
                input.CreateUserId = dictionary.CreateUserId;
                input.CreateUserName = dictionary.CreateUserName;

                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;

                operateStatus = await UpdateAsync(input);
            }
            await GeneratingParentIds(input);
            return operateStatus;
        }

        /// <summary>
        /// ɾ���ֵ估�¼�����
        /// </summary>
        /// <param name="input">id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                return OperateStatus.Error();
            }
            foreach (var id in input.Id.Split(','))
            {
                //�жϸ��ֵ��Ƿ�����ɾ��:������ϵͳ������ֵ�������ɾ��
                var dictionary = await FindAsync(f => f.DictionaryId == Guid.Parse(id));
                if (!dictionary.CanbeDelete)
                {
                    return OperateStatus.Error(Chs.CanotDelete);
                }
                //�Ƿ��������
                IEnumerable<SystemDictionary> dictionaries = (await FindDictionaryByParentId(new IdInput(Guid.Parse(id)))).Data;
                if (dictionaries.Any())
                {
                    return OperateStatus.Error(ResourceSystem.�����¼���);
                }
            }
            foreach (var id in input.Id.Split(','))
            {
                try
                {
                    var operateStatus = await DeleteAsync(d => d.DictionaryId == Guid.Parse(id));
                    if (operateStatus.Code == ResultCode.Error)
                    {
                        return operateStatus;
                    }
                }
                catch (Exception e)
                {
                    return OperateStatus.Error(e.Message);
                }
            }
            return OperateStatus.Success();
        }

        #endregion

        /// <summary>
        /// �����������¼���ϵ�ֶ�����
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> GeneratingParentIds(SystemDictionary dictionary)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                //��ȡ�����ֵ���
                var dics = (await FindAllAsync()).ToList();
                var topDics = dics.FirstOrDefault(w => w.DictionaryId == dictionary.ParentId);
                if (topDics != null)
                {
                    dictionary.ParentIds = topDics.ParentIds.IsNullOrEmpty()
                        ? dictionary.DictionaryId.ToString()
                        : topDics.ParentIds + "," + dictionary.DictionaryId;
                    dictionary.ParentIdsName = topDics.ParentIdsName.IsNullOrEmpty()
                        ? dictionary.Name
                        : topDics.ParentIdsName + "/" + dictionary.Name;
                }
                else
                {
                    dictionary.ParentIds = dictionary.DictionaryId.ToString();
                    dictionary.ParentIdsName = dictionary.Name;
                }
                await UpdateAsync(dictionary);
                await GeneratingParentIds(dictionary, dics);
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
                return operateStatus;
            }
            operateStatus.Msg = Chs.Successful;
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// �ݹ��ȡ����
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="dictionaries"></param>
        private async Task GeneratingParentIds(SystemDictionary dictionary,
            IList<SystemDictionary> dictionaries)
        {
            string parentIds = dictionary.ParentIds;
            string parentIdsName = dictionary.ParentIdsName;
            var nextDic = dictionaries.Where(w => w.ParentId == dictionary.DictionaryId).ToList();
            foreach (var dic in nextDic)
            {
                dic.ParentIds = parentIds + "," + dic.DictionaryId;
                dic.ParentIdsName = parentIdsName + "/" + dic.Name;
                dic.ParentName = dictionary.Name;
                await UpdateAsync(dic);
                await GeneratingParentIds(dic, dictionaries);
            }
        }

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemDictionaryEditOutput>> FindById(IdInput input)
        {
            var data = (await FindAsync(f => f.DictionaryId == input.Id)).MapTo<SystemDictionary, SystemDictionaryEditOutput>();
            return OperateStatus<SystemDictionaryEditOutput>.Success(data);
        }

        /// <summary>
        ///    ���ݸ���id��ȡ�¼�(ֻ��һ��)
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDictionary>>> FindByParentId(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemDictionary>>.Success((await FindAllAsync(f => f.ParentId == input.Id && f.IsFreeze == false)).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.DictionaryId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// ����ParentIds��ȡ�����¼�
        /// </summary>
        /// <param name="input">����ֵ</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindByParentIds(SystemDictionaryFindByParentIdInput input)
        {
            var parentIds = input.HaveSelf ? input.Id.ToString() : input.Id + ",";
            var dics = (input.Id != null ?
                await FindAllAsync(f => f.ParentIds.Contains(parentIds)) :
                await FindAllAsync()).OrderBy(o => o.OrderNo);
            List<BaseTree> trees = new List<BaseTree>();
            foreach (var item in dics)
            {
                BaseTree baseTree = new BaseTree();
                baseTree.parents = item.ParentIds;
                baseTree.id = item.DictionaryId;
                baseTree.parent = (!input.HaveSelf ? item.ParentId.ToString() : item.DictionaryId.ToString()) == input.Id.ToString() ? null : item.ParentId;
                baseTree.text = item.Name;
                trees.Add(baseTree);
            }

            return OperateStatus<IEnumerable<BaseTree>>.Success(trees);
        }
    }
}