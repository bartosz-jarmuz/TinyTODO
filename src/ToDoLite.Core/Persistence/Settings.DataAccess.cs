using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Persistence
{
    public partial class Settings
    {
        private class DataAccess
        {
            private readonly ToDoLiteDbContext _dbContext;

            public DataAccess(ToDoLiteDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public T Get<T>(string key, T defaultValue) 
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
                    return defaultValue;
                }

                return Type.GetTypeCode(typeof(T)) switch
                {
                    TypeCode.Boolean => ValueOrDefault(setting.ValueBoolean),
                    TypeCode.Double => ValueOrDefault(setting.ValueDouble),
                    TypeCode.Int32 => ValueOrDefault(setting.ValueInteger),
                    TypeCode.DateTime => ValueOrDefault(setting.ValueDateTime),
                    TypeCode.String => ValueOrDefault(setting.ValueString ?? ""),
                    _ => throw new ArgumentOutOfRangeException($"{typeof(T)} is not supported as setting.")
                };

                T ValueOrDefault(object? value)
                {
                    if (value == null) 
                        return defaultValue;
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
