// Copyright 2010 Chris Patterson
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
namespace Stact.Specs.Actors.PingPongDemo
{
	
	using Internal;


	public class PongActor :
		Pong
	{
		private int _pingCount;
		protected Fiber _fiber;

		public PongActor()
		{
			_fiber = new PoolFiber();
		}

		public void Ping(Ping ping)
		{
			_fiber.Add(() => Consume(ping));
		}

		private void Consume(Ping ping)
		{
			ping.Pong(this);
			_pingCount++;
		}
	}
}