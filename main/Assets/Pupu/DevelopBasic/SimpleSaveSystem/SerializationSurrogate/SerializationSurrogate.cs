using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
public class ColorSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context){
        Color color = (Color)obj;
        info.AddValue("r", color.r);
        info.AddValue("g", color.g);
        info.AddValue("b", color.b);
        info.AddValue("a", color.a);
    }
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector){
        Color color = (Color)obj;
        color.r = (float)info.GetValue("r", typeof(float));
        color.g = (float)info.GetValue("g", typeof(float));
        color.b = (float)info.GetValue("b", typeof(float));
        color.a = (float)info.GetValue("a", typeof(float));

        obj = color;
        return obj;
    }
}
public class QuaternionSerializationSurrogate: ISerializationSurrogate{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context){
        Quaternion quaternion = (Quaternion)obj;
        info.AddValue("x", quaternion.x);
        info.AddValue("y", quaternion.y);
        info.AddValue("z", quaternion.z);
        info.AddValue("w", quaternion.w);

    }
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector){
        Quaternion quaternion = (Quaternion)obj;
        quaternion.x = (float)info.GetValue("x", typeof(float));
        quaternion.y = (float)info.GetValue("y", typeof(float));
        quaternion.z = (float)info.GetValue("z", typeof(float));
        quaternion.w = (float)info.GetValue("w", typeof(float));        
        obj = quaternion;
        return obj;
    }
}
public class Vector3SerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context){
        Vector3 vec = (Vector3)obj;
        info.AddValue("x", vec.x);
        info.AddValue("y", vec.y);
        info.AddValue("z", vec.z);

    }
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector){
        Vector3 vec = (Vector3)obj;
        vec.x = (float)info.GetValue("x", typeof(float));
        vec.y = (float)info.GetValue("y", typeof(float));
        vec.z = (float)info.GetValue("z", typeof(float));
        obj = vec;
        return obj;
    }
}