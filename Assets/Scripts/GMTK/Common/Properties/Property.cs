using System;

namespace GMTK.Common.Properties {
    [Serializable]
    public abstract class Property {
    }

    [Serializable]
    public abstract class Property<T> : Property {
    }

    [Serializable]
    public abstract class ObservableProperty<T> : Property {
    }
}