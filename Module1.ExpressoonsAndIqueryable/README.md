                                                    DESCRIPTION OF TASKS

Task1. 
Используя возможность конструировать Expression Tree и выполнять его код, создайте собственный механизм маппинга (копирующего поля (свойства) одного класса в другой).
Приблизительный интерфейс и пример использования приведен ниже (MapperGenerator – фабрика мапперов, Mapper – класс маппинга). Обратите внимание, что в данном примере создается только новый экземпляр конечного класса, но сами данные не копируются.

    public class Mapper<TSource, TDestination>
    {
        Func<TSource, TDestination> mapFunction;
        internal Mapper(Func<TSource, TDestination> func)
        {
            mapFunction = func;
        }
        public TDestination Map(TSource source)
        {
            return mapFunction(source);
        }
    }
    
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var mapFunction = 
                Expression.Lambda<Func<TSource, TDestination>>(
                Expression.New(typeof(TDestination)),
                sourceParam
                );
 
            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }
    }
    public class Foo { }
    public class Bar { }

Task2.
Создайте класс-трансформатор на основе ExpressionVisitor, выполняющий следующие 2 вида преобразований дерева выражений: 

a. Замену выражений вида <переменная> + 1 / <переменная> - 1 на операции инкремента и декремента.

b. Замену параметров, входящих в lambda-выражение, на константы (в качестве параметров такого преобразования передавать: 
  - Исходное выражение 
  - Список пар <имя параметра: значение для замены> 

Для контроля полученное дерево выводить в консоль или смотреть результат под отладчиком, использую ExpressionTreeVisualizer, 
а также компилировать его и вызывать полученный метод

Task3.
Доработайте приведенный на лекции LINQ провайдер. В частности, требуется добавить следующее: 
1. Снять текущее ограничение на порядок операндов выражения. Должны быть допустимы: 
    - <имя фильтруемого поля> == <константа> (сейчас доступен только этот) 
    - <константа> == <имя фильтруемого поля> 
2. Добавить поддержку операций включения (т.е. не точное совпадение со строкой, а частичное). При этом в LINQ-нотации они должны 
выглядеть как обращение к методам класса string: StartsWith, EndsWith, Contains, а точнее: 
    - Выражение: Where(e => e.workstation.StartsWith("EPRUIZHW006")), транслируется в запрос: workstation:(EPRUIZHW006*) 
    - Выражение: Where(e => e.workstation.EndsWith("IZHW0060")), транслируется в запрос: workstation:(*IZHW0060)
    - Выражение: Where(e => e.workstation.Contains("IZHW006")), транслируется в запрос: workstation:(*IZHW006*)
 3. Добавить поддержку оператора AND (потребует доработки также самого E3SQueryClient).

Task4.
Implement the function's body. Explain proposed solution.

    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> ToBool<T>(this Expression<Func<T, bool?>> expression)
        {
            // Implement the function's body
        }
    }
