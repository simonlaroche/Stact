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
namespace Stact.Data.Internal
{
	using System;


	public abstract class Node<T, M>
	{
		readonly Measured<T, M> _m;
		readonly M _measure;

		protected Node(Measured<T, M> m, M measure)
		{
			_m = m;
			_measure = measure;
		}

		public M Measure
		{
			get { return _measure; }
		}

		public Measured<T, M> Measured
		{
			get { return _m; }
		}

		public abstract U FoldRight<U>(Func<T, Func<U, U>> f, U z);
		public abstract U FoldLeft<U>(Func<U, Func<T, U>> f, U z);
		public abstract bool Visit(Func<T, bool> callback);

		public static Func<U, Func<Node<T, M>, U>> FoldLeft<U>(Func<U, Func<T, U>> bff)
		{
			return u => node => node.FoldLeft(bff, u);
		}

		public static Func<U, Func<Node<T, M>, U>> FoldRight<U>(Func<T, Func<U, U>> aff)
		{
			return u => node => node.FoldRight(aff, u);
		}

		public Node<U, M> Map<U>(Func<T, U> f, Measured<U, M> m)
		{
			return Match<Node<U, M>>(node => new Node2<U, M>(m, f(node.V1), f(node.V2)),
			                         node3 => new Node3<U, M>(m, f(node3.V1), f(node3.V2), f(node3.V3)));
		}

		public static Func<Node<T, M>, Node<U, M>> LiftM<U>(Func<T, U> f, Measured<U, M> m)
		{
			return node => node.Map(f, m);
		}

		public abstract Digit<T, M> ToDigit();

		public abstract U Match<U>(Func<Node2<T, M>, U> node2, Func<Node3<T, M>, U> node3);
	}
}