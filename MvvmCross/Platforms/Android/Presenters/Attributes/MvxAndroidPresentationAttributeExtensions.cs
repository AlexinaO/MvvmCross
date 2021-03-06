﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Android.App;
using MvvmCross.Logging;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Presenters.Attributes
{
    public static class MvxAndroidPresentationAttributeExtensions
    {
        private static Type GetActivityViewModelType(Type activityType)
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxViewModelTypeFinder associatedTypeFinder))
                return associatedTypeFinder.FindTypeOrNull(activityType);

            MvxLog.Instance.Trace("No view model type finder available - assuming we are looking for a splash screen - returning null");
            return typeof(MvxNullViewModel);
        }

        public static bool IsFragmentCacheable(this Type fragmentType, Type fragmentActivityParentType)
        {
            if (!fragmentType.HasBasePresentationAttribute())
                return false;

            var fragmentAttributes = fragmentType.GetBasePresentationAttributes()
                                                 .Select(baseAttribute => baseAttribute as MvxFragmentPresentationAttribute)
                                                 .Where(fragmentAttribute => fragmentAttribute != null);

            var currentAttribute = fragmentAttributes.FirstOrDefault(fragmentAttribute => fragmentAttribute.ActivityHostViewModelType == fragmentActivityParentType);

            return currentAttribute != null ? currentAttribute.IsCacheableFragment : false;
        }

        public static PopBackStackFlags ToNativePopBackStackFlags(this MvxPopBackStack mvxPopBackStack)
        {
            switch (mvxPopBackStack)
            {
                case MvxPopBackStack.None:
                    return PopBackStackFlags.None;
                case MvxPopBackStack.Inclusive:
                    return PopBackStackFlags.Inclusive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mvxPopBackStack), mvxPopBackStack, $"No matching {nameof(PopBackStackFlags)} enum is defined");
            }
        }
    }
}
