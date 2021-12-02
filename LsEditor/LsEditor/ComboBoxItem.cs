using System;

namespace RCIS
{
	/// <summary>
	/// ComboBoxItem �ṩһ��ͨ�õ���CombBox�йҽ�Item�ķ�����
	/// ����ItemObject ��ʾʵ�ʹҽӵĶ���
	/// ItemText��ʾ��ʾ���û�����˵��
	/// ItemIndex��ʾ����
	/// </summary>
	public class ComboBoxItem
	{
		public object ItemObject;
		public string ItemText="";
		public string ItemIndex="";
		public ComboBoxItem(object paramObj,string paramText,int paramIndex)
            : this(paramObj, paramText, paramIndex.ToString())
		{
			
		}
        public ComboBoxItem(object paramObj, string pText)
            :this(paramObj ,pText ,"")
        {
            
        }
        public ComboBoxItem(object paramObj, string paramText, string paramIndex)
        {
            if (paramText == null) paramText = "";            
            this.ItemObject = paramObj;
            this.ItemText = paramText;
            this.ItemIndex = paramIndex;
        }
		public override string ToString()
		{
            if (this.ItemIndex.Equals(""))
            {
                return this.ItemText;
            }else{
                string order = this.ItemIndex.PadRight(3, ' ');
                return order + " | " + this.ItemText;
            }
		}
		public override bool Equals(object obj)
		{
			if(this.ItemObject ==null)
				return false;
			if(obj is ComboBoxItem )
			{
				ComboBoxItem paramItem=obj as ComboBoxItem;
				return this.ItemObject .Equals (paramItem.ItemObject );
			}
			return false;
		}
        public override int GetHashCode()
        {
            if(this.ItemObject ==null)
                return base.GetHashCode();
            else return this.ItemObject .GetHashCode ();
        }

		
	}
}
