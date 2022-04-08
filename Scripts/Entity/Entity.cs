using System.Collections.Generic;
using System;
using Entities.Components;
using Godot;

namespace Entities
{

    /// <summary>
    /// Class Entity. It's only a container for all the <see cref="IComponentNode"/>. 
    /// <remarks>
    /// </remarks>
    /// </summary>
    public class Entity : Node2D
    {
        #region Properties
        /// <summary>
        /// A Dictionary of of all <see cref="MyComponents"/>
        /// <remarks>
        /// The use of a Dictionariy let's us only use one component of each possible type.
        /// At the moment THIS IS the behaviour we want 
        /// </remarks>
        /// </summary>
        public Dictionary<Type, IComponentNode> MyComponents { get; set; }


        #endregion

        #region Constructors

        /// <summary>
        /// Empty Constructor
        /// </summary>        
        public Entity()
        {
            MyComponents = new Dictionary<Type, IComponentNode>();
        }

        #endregion


        #region Godot Methods
        /// <summary>
        /// Method called when this entity enters the tree.
        /// It sets all the components and call foreach one <see cref="IComponentNode.OnAwake"/>
        /// </summary>
        public override void _EnterTree()
        {
            this.MyComponents = new Dictionary<Type, IComponentNode>();
            // so, in order to automate this a little, on enter tree looks for the children.
            this.AddIComponentChildren(this);

        }

        /// <summary>
        /// Method called when all the next frame after entering the tree
        /// It sets all the components and call foreach one <see cref="IComponentNode.OnStart"/>
        /// </summary>
        public override void _Ready()
        {
            foreach(IComponentNode component in this.MyComponents.Values){
                component.OnStart();
            }
        }

        /// <summary>
        /// Method called when this entity exitsthe tree.
        /// It sets all the components and call foreach one <see cref="IComponentNode.OnSetFree/>
        /// </summary>
        public override void _ExitTree()
        {
            foreach(IComponentNode component in this.MyComponents.Values){
                component.OnSetFree();
            }
        }

        /// <summary>
        /// Looks throug all the children and if the childrens is a <see cref="IComponentNode"/> then adds it to
        /// <see cref="Entity.MyComponents"/> and set the <see cref="IComponentNode.MyEntity"/> as this entity
        /// </summary>
        private void AddIComponentChildren(in Node root)
        {

            IComponentNode componentNode;
            Node child;
            for (int i = 0; i < root.GetChildCount(); i++)
            {
                child = root.GetChild(i);
                componentNode = child as IComponentNode;

                if(child != null){
                    this.TryAddIComponentNode(componentNode);
                }

                //de cada objeto miraremos si sus hijos son también mierda de estas
                this.AddIComponentChildren(child);
                
            }
        }
        #endregion

        #region List methods
        /// <summary>
        /// Addes a <see cref="IComponentNode"/> to <see cref="MyComponents"/> if the list doesn't contain it with a provided generic 
        /// </summary>
        /// <param name="component">The component to add</param>
        /// <returns>Is the component succefully added?</returns>
        public bool TryAddIComponentNode<T>(in IComponentNode component) where T : IComponentNode
        {
            Type myType = typeof(T);

            if (MyComponents.ContainsKey(myType) == false)
            {
                this.AddIComponentToDictionary(component, myType);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Addes a <see cref="IComponentNode"/> to <see cref="MyComponents"/> if the list doesn't contain it
        /// </summary>
        /// <param name="component">The component to add</param>
        /// <returns>Is the component succefully added?</returns>
        public bool TryAddIComponentNode(in IComponentNode component)
        {
            Type type = component.GetType();

            if (component != null && this.MyComponents.ContainsKey(type) == false)
            {
                this.AddIComponentToDictionary(component, type);
                return true;
            }

            return false;
        }

        private void AddIComponentToDictionary(in IComponentNode component, in Type type)
        {
            this.MyComponents.Add(type, component);
            component.OnAwake();
            component.MyEntity = this;
        }

        /// <summary>
        /// Gets a <see cref="IComponentNode"/> of the entity
        /// </summary>
        /// <typeparam name="T">The type of the component to get</typeparam>
        /// <param name="component">The out component</param>
        /// <returns>Got The component?</returns>
        public bool TryGetIComponentNode<T>(out T component) where T : class
        {
            component = null;

            if (MyComponents == null)
            {
                return false;
            }

            IComponentNode c;

            MyComponents.TryGetValue(typeof(T), out c);

            if (c != null)
            {
                component = c as T;
                return true;
            }

            return false;

        }
        /// <summary>
        /// Remove a <see cref="IComponentNode"/> by its type if exist inside <see cref="MyComponents"/>
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IComponentNode"/></typeparam>
        /// <returns>Is the component succefully removed?</returns>
        public bool TryRemoveIComponentNode<T>() where T : IComponentNode
        {
            if (MyComponents == null)
            {
                return false;
            }

            if (MyComponents.ContainsKey(typeof(T)))
            {
                MyComponents.Remove(typeof(T));
                return true;
            }


            return false;
        }

        /// <summary>
        /// Removes a <see ref="IComponentNode"/> if exist, else, returns false
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryRemoveIComponentNode(in IComponentNode component)
        {
            if (MyComponents == null)
            {
                return false;
            }

            if (MyComponents.ContainsKey(component.GetType()))
            {
                MyComponents.Remove(component.GetType());
                return true;
            }

            return false;
        }
        #endregion        

        /// <summary>
        /// To call when destroy the entity
        /// </summary>
        public void FreeComponents()
        {
            if (MyComponents == null)
            {
                return;
            }
            foreach (IComponentNode c in MyComponents.Values)
            {
                c.OnSetFree();
            }

            MyComponents.Clear();
        }

        /// <summary>
        /// Resets the components
        /// </summary>
        public void ResetComponents()
        {
            foreach (Type c in MyComponents.Keys)
            {
                MyComponents[c].Reset();
            }
        }

    }



}