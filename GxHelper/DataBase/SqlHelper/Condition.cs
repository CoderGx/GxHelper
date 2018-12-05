﻿using GxHelper.AttributeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxHelper.DataBase.SqlHelper
{
    public class Condition
    {
        public Condition(SqlHelper sqlGelper, Join join)
        {
            this._SqlHelper = sqlGelper;
            this._Join = join;
        }
        Condition _ConditionLeft { get; set; }
        Connector _Connector { get; set; }
        Condition _ConditionRight { get; set; }
        SqlHelper _SqlHelper { get; set; }
        Join _Join { get; set; }
        bool IsBase
        {
            get
            {
                return _ConditionBase != null;
            }
        }
        internal ConditionSingle _ConditionBase { get; set; }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            if (IsBase)
            {
                str.Append(_ConditionBase.ToString());
            }
            else
            {
                str.AppendFormat(" ( {0} {1} {2} ) ", _ConditionLeft.ToString(), EnumHelper.GetEnumContent(_Connector) , _ConditionRight.ToString());
            }
            return str.ToString();
        }
        public Condition AddAndCondition(string _FieldName, Comparison _Comparison, string _Param=null,bool isValue=true)
        {
            return AddCondition(Connector.And, new ConditionSingle(_FieldName, _Comparison, _Param, isValue));
        }
        public Condition AddOrCondition(string _FieldName, Comparison _Comparison, string _Param = null, bool isValue = true)
        {
            return AddCondition(Connector.Or, new ConditionSingle(_FieldName, _Comparison, _Param, isValue));
        }
        Condition AddCondition(Connector connector, ConditionSingle conditionBase)
        {
            return new Condition(_SqlHelper, _Join)
            {
                _ConditionLeft = this,
                _Connector = connector,
                _ConditionRight = new Condition(_SqlHelper, _Join) { _ConditionBase = conditionBase }
            };
        }
        Condition AddCondition(Connector connector, Condition condition)
        {
            return new Condition(_SqlHelper,_Join)
            {
                _ConditionLeft = this,
                _Connector = connector,
                _ConditionRight = condition
            };
        }
        public SqlHelper EndWhere()
        {
            this._SqlHelper._Condition = this;
            return this._SqlHelper;
        }
        public SqlHelper EndJoin()
        {
            this._Join._Condition = this;
            return this._Join._SqlHelper;
        }
    }
}
