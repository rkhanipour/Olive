﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Olive.Audit
{
    public class DatabaseLogger : IAuditLogger
    {
        static Type AuditEventImplementation;

        public static void SetApplicationEventType(Type type) => AuditEventImplementation = type;

        public IAuditEvent CreateInstance()
        {
            if (AuditEventImplementation != null)
                return Activator.CreateInstance(AuditEventImplementation) as IAuditEvent;

            var possible = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a =>
            {
                try { return a.GetExportedTypes().Where(t => t.Implements<IAuditEvent>() && !t.IsInterface).ToList(); }
                catch
                {
                    // No logging is needed
                    return new List<Type>();
                }
            }).ToList();

            if (possible.None())
                throw new Exception("No type in the currently loaded assemblies implements IApplicationEvent.");

            if (possible.HasMany())
                throw new Exception($"More than one type in the currently loaded assemblies implement IApplicationEvent:{possible.Select(x => x.FullName).ToString(" and ")}");

            AuditEventImplementation = possible.Single();
            return CreateInstance();
        }

        public Task Log(IAuditEvent auditEvent) => Context.Current.Database().Save(auditEvent);
    }
}
