using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestGame.PlayingState.EventRegister;

public static class EventFactory {

    public static void InstantiateAllEvents()  {
            var assembly = Assembly.GetExecutingAssembly();
            var eventTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes(typeof(RegisterEventAttribute), false).Length > 0);


            foreach (var unused in eventTypes.Select(Activator.CreateInstance)) { }
    }
}
