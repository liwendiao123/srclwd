﻿using Microsoft.EntityFrameworkCore;
using Senparc.Core.Enums;
using Senparc.Core.Models;
using Senparc.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Senparc.Service
{
    public class BaseService<T> : BaseServiceData, IBaseService<T> where T : class, new()// global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        public IBaseRepository<T> BaseRepository { get; set; }

        public BaseService(IBaseRepository<T> repo)
            : base(repo)
        {
            BaseRepository = repo;
        }

        public virtual bool IsInsert(T obj)
        {
            return BaseRepository.IsInsert(obj);
        }

        public T GetObject(Expression<Func<T, bool>> where, string[] includes = null)
        {
            return BaseRepository.GetFirstOrDefaultObject(where, includes);
        }

        public T GetObject<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return BaseRepository.GetFirstOrDefaultObject(where, orderBy, orderingType, includes);
        }

        public PagedList<T> GetFullList<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return this.GetObjectList(0, 0, where, orderBy, orderingType, includes);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">每页数量</param>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="orderingType">正序|倒叙</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual PagedList<T> GetObjectList<TK>(int pageIndex, int pageCount, Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return BaseRepository.GetObjectList(where, orderBy, orderingType, pageIndex, pageCount, includes);
        }


        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">每页数量</param>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="orderingType">正序|倒叙</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<T>> GetObjectListAsync<TK>(int pageIndex, int pageCount, Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return await BaseRepository.GetObjectListAsync(where, orderBy, orderingType, pageIndex, pageCount, includes);
        }

        public virtual int GetCount(Expression<Func<T, bool>> where, string[] includes = null)
        {
            return BaseRepository.ObjectCount(where, includes);
        }

        public virtual decimal GetSum(Expression<Func<T, bool>> where, Func<T, decimal> sum, string[] includes = null)
        {
            return BaseRepository.GetSum(where, sum, includes);
        }

        /// <summary>
        /// 强制将实体设置为Modified状态
        /// </summary>
        /// <param name="obj"></param>
        public virtual void TryDetectChange(T obj)
        {
            if (!IsInsert(obj))
            {
                BaseRepository.BaseDB.BaseDataContext.Entry(obj).State = EntityState.Modified;
            }
        }

        public virtual void SaveObject(T obj)
        {
            if (BaseRepository.BaseDB.ManualDetectChangeObject)
            {
                TryDetectChange(obj);
            }
            BaseRepository.Save(obj);
        }

        public virtual void DeleteObject(Expression<Func<T, bool>> predicate)
        {
            T obj = GetObject(predicate);
            DeleteObject(obj);
        }

        public virtual void DeleteObject(T obj)
        {
            BaseRepository.Delete(obj);
        }

        public virtual void DeleteAll(IEnumerable<T> objects)
        {
            var list = objects.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                DeleteObject(list[i]);
            }
        }
    }
}