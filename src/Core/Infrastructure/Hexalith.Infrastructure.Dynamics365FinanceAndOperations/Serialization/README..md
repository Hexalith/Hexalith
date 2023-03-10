# Hexalith.Extensions.Serialization

Json polymorphic base class attribute. All classes that inherit from the class with this attribute will support polymorphic serialization.

```csharp
[JsonPolymorphicBaseClass]
public class MyBaseClass
{
   public string Prop1 {get; set;}
}
public class MyClass : MyBaseClass
{
   public string Prop2 {get; set;}
}
public class MyClass2 : MyBaseClass
{
   public string Prop3 {get; set;}
}
public class MyClass3 : MyClass
{
   public string Prop3 {get; set;}
}
```
