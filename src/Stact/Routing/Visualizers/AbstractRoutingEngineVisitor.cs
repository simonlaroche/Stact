﻿// Copyright 2010 Chris Patterson
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Stact.Routing.Visualizers
{
	using Actors.Internal;
	using Internal;
	using Magnum.Extensions;
	using Magnum.Reflection;

	public abstract class AbstractRoutingEngineVisitor<T> :
		ReflectiveVisitorBase<T>
		where T : AbstractRoutingEngineVisitor<T>
	{
		protected virtual bool Visit(DynamicRoutingEngine engine)
		{
			Visit(engine.Router);
			return true;
		}

		protected virtual bool Visit(TypeRouter channel)
		{
			channel.Activations.Each(typeChannel => Visit(typeChannel));
			return true;
		}

		protected virtual bool Visit<TChannel>(AlphaNode<TChannel> node)
		{
			node.Successors.Each(activation => Visit(activation));
			return true;
		}

		protected virtual bool Visit<TChannel>(JoinNode<TChannel> node)
		{
			node.Activations.Each(activation => Visit(activation));
			Visit(node.RightActivation);
			return true;
		}

		protected virtual bool Visit<T1,T2>(JoinNode<T1,T2> node)
		{
			node.Activations.Each(activation => Visit(activation));
			Visit(node.RightActivation);
			return true;
		}

		protected virtual bool Visit<TChannel>(ConstantNode<TChannel> node)
		{
			return true;
		}

		protected virtual bool Visit<TChannel>(ConsumerNode<TChannel> node)
		{
			return true;
		}

		protected virtual bool Visit(ChannelAdapter adapter)
		{
			Visit(adapter.Output);
			return true;
		}

		protected virtual bool Visit<TActor>(ActorInbox<TActor> inbox) 
			where TActor : class, Actor
		{
			Visit(inbox.Engine);
			return true;
		}
	}
}