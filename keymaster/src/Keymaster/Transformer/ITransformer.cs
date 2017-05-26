// <copyright file="ITransformer.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     An interface for transforming properties of one object into a new object of a different type
// </summary>

namespace Keymaster.Transformer
{
    public interface ITransformer<From, To>
    {
        To Transform(From from);
    }
}
