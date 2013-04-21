using System;
using System.Reflection;
using System.Reflection.Emit;

// <Module>
internal class u003cModuleu003e
{
    static u003cModuleu003e()
    {
    }

    internal static void smethod_0(RuntimeFieldHandle f)
    {
        DynamicMethod dynamicMethod;
        FieldInfo fieldFromHandle = FieldInfo.GetFieldFromHandle(f);
        Module module = fieldFromHandle.Module;
        byte[] numArray = module.ResolveSignature(fieldFromHandle.MetadataToken);
        uint num = (uint)(numArray[(int)numArray.Length - 6] | numArray[(int)numArray.Length - 5] << 8 | numArray[(int)numArray.Length - 3] << 16 | numArray[(int)numArray.Length - 2] << -826508380 + 826508404);
        ConstructorInfo constructorInfo = module.ResolveMethod((int)(num ^ 244345315 | numArray[(int)numArray.Length - 7] << 1291540052 - 859137361 - (464992981 ^ 874288869 - (-862281059 + 1695492618)))) as ConstructorInfo;
        ParameterInfo[] parameters = constructorInfo.GetParameters();
        Type[] parameterType = new Type[(int)parameters.Length];
        for (int i = 0; i < (int)parameters.Length; i++)
        {
            parameterType[i] = parameters[i].ParameterType;
        }
        dynamicMethod = (constructorInfo.DeclaringType.IsInterface || constructorInfo.DeclaringType.IsArray ? new DynamicMethod("", constructorInfo.DeclaringType, parameterType, fieldFromHandle.DeclaringType, true) : new DynamicMethod("", constructorInfo.DeclaringType, parameterType, constructorInfo.DeclaringType, true));
        ILGenerator lGenerator = dynamicMethod.GetILGenerator();
        for (int j = 0; j < (int)parameterType.Length; j++)
        {
            lGenerator.Emit(OpCodes.Ldarg_S, j);
        }
        lGenerator.Emit(OpCodes.Newobj, constructorInfo);
        lGenerator.Emit(OpCodes.Ret);
        fieldFromHandle.SetValue(null, dynamicMethod.CreateDelegate(fieldFromHandle.FieldType));
    }

    internal static void smethod_1(RuntimeFieldHandle f)
    {
        DynamicMethod dynamicMethod;
        FieldInfo fieldFromHandle = FieldInfo.GetFieldFromHandle(f);
        Module module = fieldFromHandle.Module;
        byte[] numArray = module.ResolveSignature(fieldFromHandle.MetadataToken);
        uint num = (uint)(numArray[(int)numArray.Length - 6] | numArray[(int)numArray.Length - 5] << 8 | numArray[(int)numArray.Length - 3] << 16 | numArray[(int)numArray.Length - 2] << 2016246478 - 2016246454);
        MethodInfo methodInfo = module.ResolveMethod((int)(num ^ 447612184 | numArray[(int)numArray.Length - 7] << 556853358 - (-847142203 + 1403995537))) as MethodInfo;
        if (methodInfo.IsStatic)
        {
            fieldFromHandle.SetValue(null, Delegate.CreateDelegate(fieldFromHandle.FieldType, methodInfo));
            return;
        }
        string name = fieldFromHandle.Name;
        ParameterInfo[] parameters = methodInfo.GetParameters();
        Type[] parameterType = new Type[(int)parameters.Length + 1];
        parameterType[0] = typeof(object);
        for (int i = 0; i < (int)parameters.Length; i++)
        {
            parameterType[i + 1] = parameters[i].ParameterType;
        }
        Type declaringType = methodInfo.DeclaringType;
        Type type = fieldFromHandle.DeclaringType;
        dynamicMethod = (declaringType.IsInterface || declaringType.IsArray ? new DynamicMethod("", methodInfo.ReturnType, parameterType, type, true) : new DynamicMethod("", methodInfo.ReturnType, parameterType, declaringType, true));
        ILGenerator lGenerator = dynamicMethod.GetILGenerator();
        for (int j = 0; j < (int)parameterType.Length; j++)
        {
            lGenerator.Emit(OpCodes.Ldarg, j);
            if (j == 0)
            {
                lGenerator.Emit(OpCodes.Castclass, declaringType);
            }
        }
        if (name[0] != '\u0002')
        {
            lGenerator.Emit(OpCodes.Call, methodInfo);
        }
        else
        {
            lGenerator.Emit(OpCodes.Callvirt, methodInfo);
        }
        lGenerator.Emit(OpCodes.Ret);
        fieldFromHandle.SetValue(null, dynamicMethod.CreateDelegate(fieldFromHandle.FieldType));
    }
}

 