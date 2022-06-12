using TinyTODO.Core.DataModel;

namespace TinyTODO.Core
{
    public partial class Settings
    {
        private class DataAccess
        {
            private readonly TinyToDoDbContext _dbContext;

            public DataAccess(TinyToDoDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public T Get<T>(string key) 
            {
                var setting = _dbContext.Settings.Find(key);
                
                if (setting == null)
                {
                    setting = new Setting()
                    {
                        Key = key,
                    };
                    _dbContext.Settings.Add(setting);
                    _dbContext.SaveChanges();
                    return default;
                }
                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Boolean:
                        return ValueOrDefault(setting.ValueBoolean);
                    case TypeCode.Double:
                        return ValueOrDefault(setting.ValueDouble);
                    case TypeCode.Int32:
                        return ValueOrDefault(setting.ValueInteger);
                    case TypeCode.DateTime:
                        return ValueOrDefault(setting.ValueDateTime);
                    case TypeCode.String:
                        return ValueOrDefault(setting.ValueString??"");
                    default:
                        throw new ArgumentOutOfRangeException($"{typeof(T)} is not supported as setting.");
                }

                T ValueOrDefault(object? value)
                {
                    if (value == null) 
                        return default(T);
                    else 
                        return (T)value;
                }
            }

            public void Set(string key, object value)
            {
                var setting = _dbContext.Settings.Find(key);
                if (setting == null)
                {
                    setting = new Setting()
                    {
                        Key = key
                    };
                    SetValue(setting, value);
                    _dbContext.Settings.Add(setting);
                }
                else
                {
                    SetValue(setting, value);
                    _dbContext.Settings.Update(setting);
                }
                
               _dbContext.SaveChanges();

                static void SetValue(Setting setting, object value)
                {
                    switch (value)
                    {
                        case bool recognized:
                            setting.ValueBoolean = recognized;
                            break;
                        case double recognized:
                            setting.ValueDouble = recognized;
                            break;
                        case int recognized:
                            setting.ValueInteger = recognized;
                            break;
                        case DateTime recognized:
                            setting.ValueDateTime = recognized;
                            break;
                        case string:
                            setting.ValueString = value?.ToString() ?? "";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"{value?.GetType().Name} is not supported as setting.");
                    }

                }
            }
        }
    }
}
