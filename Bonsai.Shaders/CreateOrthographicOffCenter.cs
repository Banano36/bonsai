﻿using OpenTK;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.Shaders
{
    /// <summary>
    /// Represents an operator that creates an orthographic projection matrix
    /// from specified projection volume boundaries.
    /// </summary>
    [Description("Creates an orthographic projection matrix from specified projection volume boundaries.")]
    public class CreateOrthographicOffCenter : Source<Matrix4>
    {
        /// <summary>
        /// Gets or sets the left edge of the projection volume.
        /// </summary>
        [Description("The left edge of the projection volume.")]
        public float Left { get; set; } = -1;

        /// <summary>
        /// Gets or sets the right edge of the projection volume.
        /// </summary>
        [Description("The right edge of the projection volume.")]
        public float Right { get; set; } = 1;

        /// <summary>
        /// Gets or sets the bottom edge of the projection volume.
        /// </summary>
        [Description("The bottom edge of the projection volume.")]
        public float Bottom { get; set; } = -1;

        /// <summary>
        /// Gets or sets the top edge of the projection volume.
        /// </summary>
        [Description("The top edge of the projection volume.")]
        public float Top { get; set; } = 1;

        /// <summary>
        /// Gets or sets the distance to the near clip plane.
        /// </summary>
        [Category("Z-Clipping")]
        [Description("The distance to the near clip plane.")]
        public float NearClip { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets the distance to the far clip plane.
        /// </summary>
        [Category("Z-Clipping")]
        [Description("The distance to the far clip plane.")]
        public float FarClip { get; set; } = 1000f;

        /// <summary>
        /// Generates an observable sequence that returns a 4x4 orthographic
        /// projection matrix with the specified parameters.
        /// </summary>
        /// <returns>
        /// A sequence containing the created <see cref="Matrix4"/> object.
        /// </returns>
        public override IObservable<Matrix4> Generate()
        {
            return Observable.Defer(() => Observable.Return(Matrix4.CreateOrthographicOffCenter(Left, Right, Bottom, Top, NearClip, FarClip)));
        }

        /// <summary>
        /// Generates an observable sequence of orthographic matrices with the
        /// specified parameters, where each <see cref="Matrix4"/> object is
        /// emitted only when an observable sequence emits a notification.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting new matrices.
        /// </param>
        /// <returns>
        /// The sequence of created <see cref="Matrix4"/> values.
        /// </returns>
        public IObservable<Matrix4> Generate<TSource>(IObservable<TSource> source)
        {
            return source.Select(x => Matrix4.CreateOrthographicOffCenter(Left, Right, Bottom, Top, NearClip, FarClip));
        }
    }
}
