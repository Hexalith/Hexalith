using System;
using System.Runtime.Serialization;

namespace Hexalith.Infrastructure.VisualComponents.Exceptions
{
    [Serializable]
    internal class DuplicateComponentRendererException : Exception
    {
        public DuplicateComponentRendererException()
        {
        }

        public DuplicateComponentRendererException(string? message) : base(message)
        {
        }

        public DuplicateComponentRendererException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public DuplicateComponentRendererException(string theme, Type control, Type newRenderer, Type existingRenderer, string? message = default, Exception? innerException = default)
            : this($"Renderer '{newRenderer.FullName}' can't be added. It's a dulicate of '{existingRenderer.FullName}' for control '{control.Name}' in theme '{theme}'.\n{message}", innerException)
        {
        }

        protected DuplicateComponentRendererException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}