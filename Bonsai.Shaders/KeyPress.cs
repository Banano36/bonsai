﻿using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Shaders
{
    [Description("Produces a sequence of characters whenever a key is pressed while the shader window has focus.")]
    [Editor("Bonsai.Shaders.Design.ShaderConfigurationComponentEditor, Bonsai.Shaders.Design", typeof(ComponentEditor))]
    public class KeyPress : Source<EventPattern<KeyPressEventArgs>>
    {
        public override IObservable<EventPattern<KeyPressEventArgs>> Generate()
        {
            return ShaderManager.WindowSource.SelectMany(window => Observable.FromEventPattern<KeyPressEventArgs>(
                handler => window.KeyPress += handler,
                handler => window.KeyPress -= handler));
        }
    }
}
