using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Mappings
{
    public class GuidStringUserType : IUserType
    {
        public SqlType[] SqlTypes => new[] { SqlTypeFactory.GetString(36) };

        public Type ReturnedType => typeof(Guid);

        public bool IsMutable => false;

        public object? NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var value = rs[names[0]] as string;
            return string.IsNullOrEmpty(value) ? Guid.Empty : Guid.Parse(value);
        }

        public void NullSafeSet(DbCommand cmd, object? value, int index, ISessionImplementor session)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                ((IDataParameter)cmd.Parameters[index]).Value = value.ToString();
            }
        }

        public object DeepCopy(object value) => value;

        public object Replace(object original, object target, object owner) => original;

        public object Assemble(object cached, object owner) => cached;

        public object Disassemble(object value) => value;

        public new bool Equals(object x, object y) => object.Equals(x, y);

        public int GetHashCode(object x) => x?.GetHashCode() ?? 0;
    }
}