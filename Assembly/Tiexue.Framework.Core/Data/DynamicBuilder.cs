using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Reflection.Emit;


namespace Tiexue.Framework.Data
{

    internal sealed class DynamicBuilder<T>
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("TiexueFrameworkCore");
        #endregion

        private static readonly MethodInfo getValueMethod = typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });
        private delegate T Load(IDataRecord dataRecord);
        private delegate object GetValue(T t_Model);
        private delegate T SetValue(T t_Model, object value);
        private Load handler;

        private DynamicBuilder() { }

        public T BuildEntity(IDataRecord dataRecord)
        {
            return handler(dataRecord);
        }


        private static MethodInfo CreateConverterMethodInfo(string method)
        {
            return typeof(InternalConverter).GetMethod(method, new Type[] { typeof(object) });
        }
        public static DynamicBuilder<T> CreateEntityBuilder(IDataRecord dataRecord)
        {

            Type t = typeof(T);
            DynamicBuilder<T> dynamicBuilder = new DynamicBuilder<T>();

            try
            {

                DynamicMethod method = new DynamicMethod("DynamicModelCreate", typeof(T), new Type[] { typeof(IDataRecord) }, typeof(T), true);

                ILGenerator generator = method.GetILGenerator();

                LocalBuilder result = generator.DeclareLocal(typeof(T));

                generator.Emit(OpCodes.Newobj, t.GetConstructor(Type.EmptyTypes));

                generator.Emit(OpCodes.Stloc, result);
                var properties = t.GetProperties();
                for (int i = 0; i < dataRecord.FieldCount; i++)
                {


                    var temp = properties.Where(n => n.Name.ToLower() == dataRecord.GetName(i).ToLower()).ToList();
                    PropertyInfo propertyInfo = null;

                    if (temp.Count() >= 1)
                    {
                        propertyInfo = temp[0];
                    }

                    LocalBuilder il_P = generator.DeclareLocal(typeof(PropertyInfo));

                    Label endIfLabel = generator.DefineLabel();
                    if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldc_I4, i);
                        generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                        generator.Emit(OpCodes.Brtrue, endIfLabel);

                        generator.Emit(OpCodes.Ldloc, result);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldc_I4, i);
                        generator.Emit(OpCodes.Callvirt, getValueMethod);

                        switch (propertyInfo.PropertyType.Name)
                        {
                            case "Int16":
                                generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt16", new Type[] { typeof(object) }));
                                break;
                            case "Int32":
                                generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt32", new Type[] { typeof(object) }));
                                break;
                            case "Int64":
                                generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt64", new Type[] { typeof(object) }));
                                break;
                            case "Boolean":
                                generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToBoolean", new Type[] { typeof(object) }));
                                break;
                            case "String":
                                generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToString", new Type[] { typeof(object) }));
                                break;
                            case "DateTime":
                                generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToDateTime", new Type[] { typeof(object) }));
                                break;
                            case "Decimal":
                                generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToDecimal", new Type[] { typeof(object) }));
                                break;
                            case "Byte":
                                generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToByte", new Type[] { typeof(object) }));
                                break;
                            case "Nullable`1":
                                {
                                    if (propertyInfo.PropertyType == typeof(DateTime?))
                                    {
                                        generator.Emit(OpCodes.Call, CreateConverterMethodInfo("ToDateTimeNull"));
                                    }
                                    else if (propertyInfo.PropertyType == typeof(Int32?))
                                    {
                                        generator.Emit(OpCodes.Call, CreateConverterMethodInfo("ToInt32Null"));
                                    }
                                    else if (propertyInfo.PropertyType == typeof(Boolean?))
                                    {
                                        generator.Emit(OpCodes.Call, CreateConverterMethodInfo("ToBooleanNull"));
                                    }
                                    break;
                                }
                            default:
                                generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                                break;
                        }

                        generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());

                        generator.MarkLabel(endIfLabel);
                    }
                }

                generator.Emit(OpCodes.Ldloc, result);
                generator.Emit(OpCodes.Ret);


                dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));


            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dynamicBuilder;
        }

    }
    public static class InternalConverter
    {

        public static Byte ToByte(object value)
        {
            return SafeToType<Byte>(value);
        }

        public static Byte[] ToBytes(object value)
        {
            return SafeToType<Byte[]>(value);
        }
        public static DateTime? ToDateTimeNull(object value)
        {
            return SafeToType<DateTime?>(value);
        }

        public static System.Int32? ToInt32Null(object value)
        {
            return SafeToType<Int32?>(value);
        }

        public static Boolean? ToBooleanNull(object value)
        {
            return SafeToType<Boolean?>(value);
        }

        private static T SafeToType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t); ;
            }

            return (T)Convert.ChangeType(value, t);
        }
    }

}
