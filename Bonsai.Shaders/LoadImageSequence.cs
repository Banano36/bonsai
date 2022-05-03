﻿using Bonsai.Shaders.Configuration;
using OpenCV.Net;
using OpenTK.Graphics.OpenGL4;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.Shaders
{
    /// <summary>
    /// Represents an operator that loads an image texture sequence from the
    /// specified movie file or image sequence pattern.
    /// </summary>
    [DefaultProperty(nameof(FileName))]
    [Description("Loads an image texture sequence from the specified movie file or image sequence pattern.")]
    public class LoadImageSequence : Source<Texture>
    {
        readonly UpdateFrame updateFrame = new UpdateFrame();
        readonly ImageSequence configuration = new ImageSequence();

        /// <summary>
        /// Gets or sets the width of the texture. If no value is specified, the
        /// texture buffer will not be initialized.
        /// </summary>
        [Category("TextureSize")]
        [Description("The width of the texture. If no value is specified, the texture buffer will not be initialized.")]
        public int? Width
        {
            get { return configuration.Width; }
            set { configuration.Width = value; }
        }

        /// <summary>
        /// Gets or sets the height of the texture. If no value is specified, the
        /// texture buffer will not be initialized.
        /// </summary>
        [Category("TextureSize")]
        [Description("The height of the texture. If no value is specified, the texture buffer will not be initialized.")]
        public int? Height
        {
            get { return configuration.Height; }
            set { configuration.Height = value; }
        }

        /// <summary>
        /// Gets or sets a value specifying the internal pixel format of the texture.
        /// </summary>
        [Category("TextureParameter")]
        [Description("Specifies the internal pixel format of the texture.")]
        public PixelInternalFormat InternalFormat
        {
            get { return configuration.InternalFormat; }
            set { configuration.InternalFormat = value; }
        }

        /// <summary>
        /// Gets or sets a value specifying wrapping parameters for the column
        /// coordinates of the texture sampler.
        /// </summary>
        [Category("TextureParameter")]
        [Description("Specifies wrapping parameters for the column coordinates of the texture sampler.")]
        public TextureWrapMode WrapS
        {
            get { return configuration.WrapS; }
            set { configuration.WrapS = value; }
        }

        /// <summary>
        /// Gets or sets a value specifying wrapping parameters for the row
        /// coordinates of the texture sampler.
        /// </summary>
        [Category("TextureParameter")]
        [Description("Specifies wrapping parameters for the row coordinates of the texture sampler.")]
        public TextureWrapMode WrapT
        {
            get { return configuration.WrapT; }
            set { configuration.WrapT = value; }
        }

        /// <summary>
        /// Gets or sets a value specifying the texture minification filter.
        /// </summary>
        [Category("TextureParameter")]
        [Description("Specifies the texture minification filter.")]
        public TextureMinFilter MinFilter
        {
            get { return configuration.MinFilter; }
            set { configuration.MinFilter = value; }
        }

        /// <summary>
        /// Gets or sets a value specifying the texture magnification filter.
        /// </summary>
        [Category("TextureParameter")]
        [Description("Specifies the texture magnification filter.")]
        public TextureMagFilter MagFilter
        {
            get { return configuration.MagFilter; }
            set { configuration.MagFilter = value; }
        }

        /// <summary>
        /// Gets or sets the path to a movie file or image sequence search pattern.
        /// </summary>
        [Category("TextureData")]
        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        [FileNameFilter("Video Files|*.avi;*.mp4;*.ogg;*.ogv;*.wmv|AVI Files (*.avi)|*.avi|MP4 Files (*.mp4)|*.mp4|OGG Files (*.ogg;*.ogv)|*.ogg;*.ogv|WMV Files (*.wmv)|*.wmv")]
        [Description("The path to a movie file or image sequence search pattern.")]
        public string FileName
        {
            get { return configuration.FileName; }
            set { configuration.FileName = value; }
        }

        /// <summary>
        /// Gets or sets a value specifying the flip mode applied to individual frames.
        /// </summary>
        [Category("TextureData")]
        [Description("Specifies the flip mode applied to individual frames.")]
        public FlipMode? FlipMode
        {
            get { return configuration.FlipMode; }
            set { configuration.FlipMode = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of frames to include in the image sequence.
        /// </summary>
        /// <remarks>
        /// If no value is specified, all frames in the video will be loaded in the
        /// image sequence.
        /// </remarks>
        [Category("TextureData")]
        [Description("The maximum number of frames to include in the image sequence.")]
        public int? FrameCount
        {
            get { return configuration.FrameCount; }
            set { configuration.FrameCount = value; }
        }

        /// <summary>
        /// Gets or sets the offset, in frames, at which the image sequence should start.
        /// </summary>
        [Category("TextureData")]
        [Description("The offset, in frames, at which the image sequence should start.")]
        public int StartPosition
        {
            get { return configuration.StartPosition; }
            set { configuration.StartPosition = value; }
        }

        /// <summary>
        /// Generates an observable sequence that returns an image texture sequence
        /// initialized from the specified movie file or image sequence pattern.
        /// </summary>
        /// <returns>
        /// A sequence containing a single instance of the <see cref="Texture"/>
        /// class representing the image texture sequence.
        /// </returns>
        public override IObservable<Texture> Generate()
        {
            var update = updateFrame.Generate().Take(1);
            return update.Select(x => configuration.CreateResource(((ShaderWindow)x.Sender).ResourceManager));
        }

        /// <summary>
        /// Returns an image texture sequence initialized from the specified movie
        /// file or image sequence pattern whenever an observable sequence emits a
        /// notification.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used to start loading a
        /// new image texture sequence.
        /// </param>
        /// <returns>
        /// The sequence of <see cref="Texture"/> objects initialized from the
        /// specified movie file or image sequence pattern whenever the
        /// <paramref name="source"/> sequence emits a notification.
        /// </returns>
        public IObservable<Texture> Generate<TSource>(IObservable<TSource> source)
        {
            return source.SelectMany(Generate());
        }
    }
}
