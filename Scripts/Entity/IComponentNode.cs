using Godot;
using System;

namespace Entities.Components
{
    /// <summary>
    /// Interface for all the components nodes.
    /// <para>
    /// As Godot uses a true OOP, its easy to use the interface
    /// </para>
    /// </summary>
    public interface IComponentNode
    {
        /// <summary>
        /// The <see cref="Entity"/> that has this component
        /// </summary>
        Entity MyEntity { get; set; }
        //Entity MyEntity { get; set; }

        /// <summary>
        /// Ready method
        /// </summary>
        void OnStart();

        /// <summary>
        /// Enter tree method
        /// </summary>
        void OnAwake();

        /// <summary>
        /// Called when setted free
        /// </summary>
        void OnSetFree();

        void Reset();

    }
}
