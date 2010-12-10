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
namespace Stact.Workflow.Configuration
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using Magnum.Extensions;


	public class ExceptionConfiguratorImpl<TWorkflow, TInstance> :
		ExceptionConfigurator<TWorkflow, TInstance>,
		ActivityBuilderConfigurator<TWorkflow, TInstance>
		where TWorkflow : class
		where TInstance : class
	{
		readonly IList<ActivityBuilderConfigurator<TWorkflow, TInstance>> _configurators;
		readonly StateConfigurator<TWorkflow, TInstance> _stateConfigurator;

		public ExceptionConfiguratorImpl(StateConfigurator<TWorkflow, TInstance> stateConfigurator)
		{
			_stateConfigurator = stateConfigurator;

			_configurators = new List<ActivityBuilderConfigurator<TWorkflow, TInstance>>();
		}

		public void ValidateConfigurator()
		{
		}

		public void Configure(ActivityBuilder<TWorkflow, TInstance> builder)
		{
			_configurators.Each(x => x.Configure(builder));
		}

		public ExceptionConfigurator<TWorkflow, TInstance, TException> Exception<TException>()
			where TException : Exception
		{
			var configurator = new ExceptionConfiguratorImpl<TWorkflow, TInstance, TException>(_stateConfigurator);

			_configurators.Add(configurator);

			return configurator;
		}
	}


	public class ExceptionConfiguratorImpl<TWorkflow, TInstance, TException> :
		ExceptionConfigurator<TWorkflow, TInstance, TException>,
		ActivityBuilderConfigurator<TWorkflow, TInstance>
		where TWorkflow : class
		where TInstance : class
		where TException : Exception
	{
		readonly IList<ActivityBuilderConfigurator<TWorkflow, TInstance>> _configurators;
		readonly StateConfigurator<TWorkflow, TInstance> _stateConfigurator;

		public ExceptionConfiguratorImpl(StateConfigurator<TWorkflow, TInstance> stateConfigurator)
		{
			_stateConfigurator = stateConfigurator;

			_configurators = new List<ActivityBuilderConfigurator<TWorkflow, TInstance>>();
		}

		public void ValidateConfigurator()
		{
		}

		public void Configure(ActivityBuilder<TWorkflow, TInstance> builder)
		{
			var exceptionBuilder = new SimpleExceptionBuilder<TWorkflow, TInstance, TException>(builder);

			_configurators.Each(x => x.Configure(exceptionBuilder));

			builder.AddExceptionHandler(exceptionBuilder.CreateExceptionHandler());
		}

		public ExceptionConfigurator<TWorkflow, TInstance, T> Exception<T>()
			where T : Exception
		{
			var configurator = new ExceptionConfiguratorImpl<TWorkflow, TInstance, T>(_stateConfigurator);

			_configurators.Add(configurator);

			return configurator;
		}

		public void AddConfigurator(ActivityBuilderConfigurator<TWorkflow, TInstance> configurator)
		{
			_configurators.Add(configurator);
		}

		public void AddConfigurator(StateBuilderConfigurator<TWorkflow, TInstance> configurator)
		{
			_stateConfigurator.AddConfigurator(configurator);
		}


		class ConfiguratorProxy :
			ExceptionBuilderConfigurator<TWorkflow, TInstance, TException>
		{
			readonly ExceptionBuilderConfigurator<TWorkflow, TInstance> _configurator;

			public ConfiguratorProxy(ExceptionBuilderConfigurator<TWorkflow, TInstance> configurator)
			{
				_configurator = configurator;
			}

			public void ValidateConfigurator()
			{
				_configurator.ValidateConfigurator();
			}

			public void Configure(ExceptionBuilder<TWorkflow, TInstance, TException> builder)
			{
				_configurator.Configure(builder);
			}
		}
	}
}