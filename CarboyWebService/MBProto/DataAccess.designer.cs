﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarBoyWebservice.MBProto
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="CarBoyAuthenticationDB")]
	public partial class DataAccessDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertsessionTbl(sessionTbl instance);
    partial void UpdatesessionTbl(sessionTbl instance);
    partial void DeletesessionTbl(sessionTbl instance);
    partial void InsertsettingTbl(settingTbl instance);
    partial void UpdatesettingTbl(settingTbl instance);
    partial void DeletesettingTbl(settingTbl instance);
    partial void InsertsmsTbl(smsTbl instance);
    partial void UpdatesmsTbl(smsTbl instance);
    partial void DeletesmsTbl(smsTbl instance);
    #endregion
		
		public DataAccessDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["CarBoyAuthenticationDBConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataAccessDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataAccessDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataAccessDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataAccessDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<sessionTbl> sessionTbls
		{
			get
			{
				return this.GetTable<sessionTbl>();
			}
		}
		
		public System.Data.Linq.Table<settingTbl> settingTbls
		{
			get
			{
				return this.GetTable<settingTbl>();
			}
		}
		
		public System.Data.Linq.Table<smsTbl> smsTbls
		{
			get
			{
				return this.GetTable<smsTbl>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.sessionTbl")]
	public partial class sessionTbl : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _phone;
		
		private long _sessionID;
		
		private long _authKeyID;
		
		private string _diffKey;
		
		private string _messageIdCollection;
		
		private int _forgetRetry;
		
		private System.DateTime _regDate;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnphoneChanging(string value);
    partial void OnphoneChanged();
    partial void OnsessionIDChanging(long value);
    partial void OnsessionIDChanged();
    partial void OnauthKeyIDChanging(long value);
    partial void OnauthKeyIDChanged();
    partial void OndiffKeyChanging(string value);
    partial void OndiffKeyChanged();
    partial void OnmessageIdCollectionChanging(string value);
    partial void OnmessageIdCollectionChanged();
    partial void OnforgetRetryChanging(int value);
    partial void OnforgetRetryChanged();
    partial void OnregDateChanging(System.DateTime value);
    partial void OnregDateChanged();
    #endregion
		
		public sessionTbl()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_phone", DbType="Char(11) NOT NULL", CanBeNull=false)]
		public string phone
		{
			get
			{
				return this._phone;
			}
			set
			{
				if ((this._phone != value))
				{
					this.OnphoneChanging(value);
					this.SendPropertyChanging();
					this._phone = value;
					this.SendPropertyChanged("phone");
					this.OnphoneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sessionID", DbType="BigInt NOT NULL")]
		public long sessionID
		{
			get
			{
				return this._sessionID;
			}
			set
			{
				if ((this._sessionID != value))
				{
					this.OnsessionIDChanging(value);
					this.SendPropertyChanging();
					this._sessionID = value;
					this.SendPropertyChanged("sessionID");
					this.OnsessionIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_authKeyID", DbType="BigInt NOT NULL")]
		public long authKeyID
		{
			get
			{
				return this._authKeyID;
			}
			set
			{
				if ((this._authKeyID != value))
				{
					this.OnauthKeyIDChanging(value);
					this.SendPropertyChanging();
					this._authKeyID = value;
					this.SendPropertyChanged("authKeyID");
					this.OnauthKeyIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_diffKey", DbType="NVarChar(55) NOT NULL", CanBeNull=false)]
		public string diffKey
		{
			get
			{
				return this._diffKey;
			}
			set
			{
				if ((this._diffKey != value))
				{
					this.OndiffKeyChanging(value);
					this.SendPropertyChanging();
					this._diffKey = value;
					this.SendPropertyChanged("diffKey");
					this.OndiffKeyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_messageIdCollection", DbType="NVarChar(800) NOT NULL", CanBeNull=false)]
		public string messageIdCollection
		{
			get
			{
				return this._messageIdCollection;
			}
			set
			{
				if ((this._messageIdCollection != value))
				{
					this.OnmessageIdCollectionChanging(value);
					this.SendPropertyChanging();
					this._messageIdCollection = value;
					this.SendPropertyChanged("messageIdCollection");
					this.OnmessageIdCollectionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_forgetRetry", DbType="Int NOT NULL")]
		public int forgetRetry
		{
			get
			{
				return this._forgetRetry;
			}
			set
			{
				if ((this._forgetRetry != value))
				{
					this.OnforgetRetryChanging(value);
					this.SendPropertyChanging();
					this._forgetRetry = value;
					this.SendPropertyChanged("forgetRetry");
					this.OnforgetRetryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_regDate", DbType="DateTime NOT NULL")]
		public System.DateTime regDate
		{
			get
			{
				return this._regDate;
			}
			set
			{
				if ((this._regDate != value))
				{
					this.OnregDateChanging(value);
					this.SendPropertyChanging();
					this._regDate = value;
					this.SendPropertyChanged("regDate");
					this.OnregDateChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.settingTbl")]
	public partial class settingTbl : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _title;
		
		private string _value;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OntitleChanging(string value);
    partial void OntitleChanged();
    partial void OnvalueChanging(string value);
    partial void OnvalueChanged();
    #endregion
		
		public settingTbl()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_title", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string title
		{
			get
			{
				return this._title;
			}
			set
			{
				if ((this._title != value))
				{
					this.OntitleChanging(value);
					this.SendPropertyChanging();
					this._title = value;
					this.SendPropertyChanged("title");
					this.OntitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_value", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string value
		{
			get
			{
				return this._value;
			}
			set
			{
				if ((this._value != value))
				{
					this.OnvalueChanging(value);
					this.SendPropertyChanging();
					this._value = value;
					this.SendPropertyChanged("value");
					this.OnvalueChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.smsTbl")]
	public partial class smsTbl : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _phone;
		
		private string _hash;
		
		private string _code;
		
		private System.DateTime _regDate;
		
		private int _retry;
		
		private int _floodTime;
		
		private byte _retryCheck;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnphoneChanging(string value);
    partial void OnphoneChanged();
    partial void OnhashChanging(string value);
    partial void OnhashChanged();
    partial void OncodeChanging(string value);
    partial void OncodeChanged();
    partial void OnregDateChanging(System.DateTime value);
    partial void OnregDateChanged();
    partial void OnretryChanging(int value);
    partial void OnretryChanged();
    partial void OnfloodTimeChanging(int value);
    partial void OnfloodTimeChanged();
    partial void OnretryCheckChanging(byte value);
    partial void OnretryCheckChanged();
    #endregion
		
		public smsTbl()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_phone", DbType="Char(11) NOT NULL", CanBeNull=false)]
		public string phone
		{
			get
			{
				return this._phone;
			}
			set
			{
				if ((this._phone != value))
				{
					this.OnphoneChanging(value);
					this.SendPropertyChanging();
					this._phone = value;
					this.SendPropertyChanged("phone");
					this.OnphoneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_hash", DbType="Char(40) NOT NULL", CanBeNull=false)]
		public string hash
		{
			get
			{
				return this._hash;
			}
			set
			{
				if ((this._hash != value))
				{
					this.OnhashChanging(value);
					this.SendPropertyChanging();
					this._hash = value;
					this.SendPropertyChanged("hash");
					this.OnhashChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_code", DbType="Char(5) NOT NULL", CanBeNull=false)]
		public string code
		{
			get
			{
				return this._code;
			}
			set
			{
				if ((this._code != value))
				{
					this.OncodeChanging(value);
					this.SendPropertyChanging();
					this._code = value;
					this.SendPropertyChanged("code");
					this.OncodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_regDate", DbType="DateTime NOT NULL")]
		public System.DateTime regDate
		{
			get
			{
				return this._regDate;
			}
			set
			{
				if ((this._regDate != value))
				{
					this.OnregDateChanging(value);
					this.SendPropertyChanging();
					this._regDate = value;
					this.SendPropertyChanged("regDate");
					this.OnregDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_retry", DbType="Int NOT NULL")]
		public int retry
		{
			get
			{
				return this._retry;
			}
			set
			{
				if ((this._retry != value))
				{
					this.OnretryChanging(value);
					this.SendPropertyChanging();
					this._retry = value;
					this.SendPropertyChanged("retry");
					this.OnretryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_floodTime", DbType="Int NOT NULL")]
		public int floodTime
		{
			get
			{
				return this._floodTime;
			}
			set
			{
				if ((this._floodTime != value))
				{
					this.OnfloodTimeChanging(value);
					this.SendPropertyChanging();
					this._floodTime = value;
					this.SendPropertyChanged("floodTime");
					this.OnfloodTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_retryCheck", DbType="TinyInt NOT NULL")]
		public byte retryCheck
		{
			get
			{
				return this._retryCheck;
			}
			set
			{
				if ((this._retryCheck != value))
				{
					this.OnretryCheckChanging(value);
					this.SendPropertyChanging();
					this._retryCheck = value;
					this.SendPropertyChanged("retryCheck");
					this.OnretryCheckChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591