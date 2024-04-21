using CodeMechanic.Diagnostics;
using evantage.Models;

namespace evantage.Pages;

public class Todoist
{
}

public static class TodoistNeo4jExtensions
{
    public static Node<T> ToNeo4jNode<T>(this T instance, bool debug = false) where T : class
    {
        return new Node<T>();
    }

    public static Relationship<T> ToNeo4jRelationship<T>(this T instance, bool debug = false) where T : class
    {
        return new Relationship<T>().Dump();
    }
}