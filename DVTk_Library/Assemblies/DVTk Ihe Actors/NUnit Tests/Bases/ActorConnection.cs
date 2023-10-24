// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>

using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

namespace Dvtk.IheActors.Bases
{
    /// <summary>
    /// ActorConnection NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class ActorConnection_NUnit
    {
        /// <summary>
        /// Contains NUnit Test Cases.
        /// </summary>
        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Dispose()
        {
        }

        /// <summary>
        ///     Test the class construction.
        /// </summary>
        [Test]
        public void TestClassConstruction()
        {
            ActorConnection actorConnection = new ActorConnection(new ActorName(ActorTypeEnum.AcquisitionModality, "ID1"), false);
            Assert.That(actorConnection.ActorName.Type, Is.EqualTo(ActorTypeEnum.AcquisitionModality));
            Assert.That(actorConnection.ActorName.Id, Is.EqualTo("ID1"));
            Assert.That(actorConnection.IsActive, Is.EqualTo(false));

            actorConnection = new ActorConnection(new ActorName(ActorTypeEnum.DssOrderFiller, "ID2"), true);
            Assert.That(actorConnection.ActorName.Type, Is.EqualTo(ActorTypeEnum.DssOrderFiller));
            Assert.That(actorConnection.ActorName.Id, Is.EqualTo("ID2"));
            Assert.That(actorConnection.IsActive, Is.EqualTo(true));
        }
    }
}
