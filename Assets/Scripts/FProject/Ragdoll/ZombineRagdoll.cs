using UnityEngine;
using System.Collections;
using System;

namespace FProject
{
    public class ZombineRagdoll
    {
        /// <summary>
        /// 骨骼信息
        /// </summary>
        private class BoneInfo
        {
            public string name;

            public Transform anchor;

            public CharacterJoint joint;

            public ZombineRagdoll.BoneInfo parent;

            public Joint joints;

            public Rigidbody bodies;

            public Collider colliders;

            public float minLimit;

            public float maxLimit;

            public float swingLimit;

            public Vector3 axis;

            public Vector3 normalAxis;

            public float radiusScale;

            public Type colliderType;

            public ArrayList children = new ArrayList();

            public float density;

            public float summedMass;
        }

        #region Property

        public ZombineBase owner;

        public Transform root;

        public Transform leftHips;

        public Transform leftKnee;

        public Transform leftFoot;

        public Transform rightHips;

        public Transform rightKnee;

        public Transform rightFoot;

        public Transform leftArm;

        public Transform leftElbow;

        public Transform leftHand;

        public Transform rightArm;

        public Transform rightElbow;

        public Transform rightHand;

        public Transform middleSpine;

        public Transform head;

        public RagdollBoneType boneType;

        public float totalMass = 20f;

        public float strength;

        private Vector3 right = Vector3.right;

        private Vector3 up = Vector3.up;

        private Vector3 forward = Vector3.forward;

        private Vector3 worldRight = Vector3.right;

        private Vector3 worldUp = Vector3.up;

        private Vector3 worldForward = Vector3.forward;

        public bool flipForward;

        private ArrayList bones;

        private ZombineRagdoll.BoneInfo rootBone;
        #endregion

        #region Method

        public ZombineRagdoll(ZombineBase mOwner, RagdollBoneType btype)
        {
            this.owner = mOwner;
            //if (this.owner is ZombieVenom)
            //{
            //    this.totalMass = 30f;
            //}
            //else
            //{
            //    this.totalMass = 20f;
            //}
            this.boneType = btype;
            if (this.boneType == RagdollBoneType.HumanBone)
            {
                this.root = this.owner.transform.FindChild("bipHandler/Bip01").transform;
                this.leftHips = this.root.FindChild("Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh").transform;
                this.leftKnee = this.leftHips.FindChild("Bip01 L Calf").transform;
                this.leftFoot = this.leftKnee.FindChild("Bip01 L Foot").transform;
                this.rightHips = this.root.FindChild("Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh").transform;
                this.rightKnee = this.rightHips.FindChild("Bip01 R Calf").transform;
                this.rightFoot = this.rightKnee.FindChild("Bip01 R Foot").transform;
                this.middleSpine = this.root.FindChild("Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2").transform;
                this.head = this.middleSpine.FindChild("Bip01 Neck/Bip01 Head").transform;
                this.leftArm = this.middleSpine.FindChild("Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm").transform;
                this.leftElbow = this.leftArm.FindChild("Bip01 L Forearm").transform;
                this.leftHand = this.leftElbow.FindChild("Bip01 L Hand/Bip01 L Finger1/Bip01 L Finger11/Bip01 L Finger1Nub").transform;
                this.rightArm = this.middleSpine.FindChild("Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm").transform;
                this.rightElbow = this.rightArm.FindChild("Bip01 R Forearm").transform;
                this.rightHand = this.rightElbow.FindChild("Bip01 R Hand/Bip01 R Finger1/Bip01 R Finger11/Bip01 R Finger1Nub").transform;
            }
            else if (this.boneType == RagdollBoneType.FlyHead)
            {
                this.root = this.owner.transform.FindChild("ZM_0003_BoneRoot").transform;
                this.leftHips = this.root.FindChild("Bone008/Bone017").transform;
                this.rightHips = this.root.FindChild("Bone008/Bone017(mirrored)").transform;
                this.middleSpine = this.root.FindChild("Bone008/Bone029").transform;
                this.leftArm = this.root.FindChild("Bone008/Bone001/Bone002/Bone035").transform;
                this.rightArm = this.root.FindChild("Bone008/Bone001/Bone002/Bone035(mirrored)").transform;
                this.head = this.root.FindChild("Bone008/Bone001/Bone002").transform;
            }
            else if (this.boneType == RagdollBoneType.Dog)
            {
                this.root = this.owner.transform.FindChild("Bip001").transform;
                this.leftHips = this.root.FindChild("Bip001 Pelvis/Bip001 Spine/Bip001 L Thigh").transform;
                this.leftKnee = this.leftHips.FindChild("Bip001 L Calf").transform;
                this.leftFoot = this.leftKnee.FindChild("Bip001 L HorseLink/Bip001 L Foot/Bip001 L Toe0").transform;
                this.rightHips = this.root.FindChild("Bip001 Pelvis/Bip001 Spine/Bip001 R Thigh").transform;
                this.rightKnee = this.rightHips.FindChild("Bip001 R Calf").transform;
                this.rightFoot = this.rightKnee.FindChild("Bip001 R HorseLink/Bip001 R Foot/Bip001 R Toe0").transform;
                this.middleSpine = this.root.FindChild("Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Spine2").transform;
                this.head = this.middleSpine.FindChild("Bip001 Neck/Bip001 Neck1/Bip001 Head").transform;
                this.leftArm = this.middleSpine.FindChild("Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm").transform;
                this.leftElbow = this.leftArm.FindChild("Bip001 L Forearm").transform;
                this.leftHand = this.leftElbow.FindChild("Bip001 L Hand/Bip001 L Finger0/Bip001 L Finger01/Bip001 L Finger0Nub").transform;
                this.rightArm = this.middleSpine.FindChild("Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm").transform;
                this.rightElbow = this.rightArm.FindChild("Bip001 R Forearm").transform;
                this.rightHand = this.rightElbow.FindChild("Bip001 R Hand/Bip001 R Finger0/Bip001 R Finger01/Bip001 R Finger0Nub").transform;
            }
            else if (this.boneType == RagdollBoneType.Desmodus)
            {
                this.root = this.owner.transform.FindChild("Bone001").transform;
                this.middleSpine = this.root.FindChild("Bone002/Bone003/Bone004").transform;
                this.head = this.middleSpine.FindChild("Bone005/Bone006/Bone007_delete").transform;
                this.leftHips = this.root.FindChild("Bone002/Bone003/Bone004/Bone016/Bone017/Bone018/Bone019").transform;
                this.leftArm = this.leftHips.FindChild("Bone020/Bone021").transform;
                this.rightHips = this.root.FindChild("Bone002/Bone003/Bone004/Bone029/Bone030/Bone031/Bone032").transform;
                this.rightArm = this.rightHips.FindChild("Bone033/Bone034").transform;
            }
            this.PrepareBones();
        }

        private void PrepareBones()
        {
            if (this.root)
            {
                this.worldRight = this.root.TransformDirection(this.right);
                this.worldUp = this.root.TransformDirection(this.up);
                this.worldForward = this.root.TransformDirection(this.forward);
            }
            if (this.bones == null)
            {
                this.bones = new ArrayList();
            }
            else
            {
                this.bones.Clear();
            }
            if (this.rootBone != null)
            {
                ((IDisposable)this.rootBone).Dispose();
            }
            this.rootBone = new ZombineRagdoll.BoneInfo();
            this.rootBone.name = "Root";
            this.rootBone.anchor = this.root;
            this.rootBone.parent = null;
            this.rootBone.density = 2.5f;
            this.bones.Add(this.rootBone);
            if (this.boneType == RagdollBoneType.HumanBone)
            {
                this.AddMirroredJoint("Hips", this.leftHips, this.rightHips, "Root", this.worldRight, this.worldForward, -20f, 70f, 30f, typeof(CapsuleCollider), 0.3f, 1.5f);
                this.AddMirroredJoint("Knee", this.leftKnee, this.rightKnee, "Hips", this.worldRight, this.worldForward, -80f, 0f, 0f, typeof(CapsuleCollider), 0.25f, 1.5f);
                this.AddMirroredJoint("Foot", this.leftFoot, this.rightFoot, "Knee", this.worldRight, this.worldForward, -90f, 0f, 0f, typeof(CapsuleCollider), 0.25f, 1.5f);
                this.AddJoint("Middle Spine", this.middleSpine, "Root", this.worldRight, this.worldForward, -20f, 20f, 10f, null, 1f, 2.5f);
                this.AddMirroredJoint("Arm", this.leftArm, this.rightArm, "Middle Spine", this.worldUp, this.worldForward, -70f, 10f, 50f, typeof(CapsuleCollider), 0.25f, 1f);
                this.AddMirroredJoint("Elbow", this.leftElbow, this.rightElbow, "Arm", this.worldForward, this.worldUp, -90f, 0f, 0f, typeof(CapsuleCollider), 0.2f, 1f);
                this.AddMirroredJoint("Hand", this.leftHand, this.rightHand, "Elbow", this.worldRight, this.worldForward, -90f, 0f, 0f, typeof(CapsuleCollider), 0.25f, 1.5f);
                this.AddJoint("Head", this.head, "Middle Spine", this.worldRight, this.worldForward, -40f, 25f, 25f, null, 1f, 1f);
            }
            else if (this.boneType == RagdollBoneType.FlyHead)
            {
                this.AddMirroredJoint("Hips", this.leftHips, this.rightHips, "Root", this.worldRight, this.worldForward, -20f, 70f, 30f, typeof(CapsuleCollider), 0.1f, 1.5f);
                this.AddJoint("Middle Spine", this.middleSpine, "Root", this.worldRight, this.worldForward, -20f, 20f, 10f, typeof(CapsuleCollider), 0.15f, 0.5f);
                this.AddJoint("Head", this.head, "Root", this.worldRight, this.worldForward, -40f, 25f, 25f, null, 1f, 1f);
            }
            else if (this.boneType == RagdollBoneType.Dog)
            {
                this.AddMirroredJoint("Hips", this.leftHips, this.rightHips, "Root", this.worldRight, this.worldForward, -20f, 70f, 30f, typeof(CapsuleCollider), 0.3f, 1.5f);
                this.AddMirroredJoint("Knee", this.leftKnee, this.rightKnee, "Hips", this.worldRight, this.worldForward, -80f, 0f, 0f, typeof(CapsuleCollider), 0.25f, 1.5f);
                this.AddMirroredJoint("Foot", this.leftFoot, this.rightFoot, "Knee", this.worldRight, this.worldForward, -90f, 0f, 0f, typeof(CapsuleCollider), 0.25f, 1.5f);
                this.AddJoint("Middle Spine", this.middleSpine, "Root", this.worldRight, this.worldForward, -20f, 20f, 10f, null, 1f, 2.5f);
                this.AddMirroredJoint("Arm", this.leftArm, this.rightArm, "Middle Spine", this.worldUp, this.worldForward, -70f, 10f, 50f, typeof(CapsuleCollider), 0.25f, 1f);
                this.AddMirroredJoint("Elbow", this.leftElbow, this.rightElbow, "Arm", this.worldForward, this.worldUp, -90f, 0f, 0f, typeof(CapsuleCollider), 0.2f, 1f);
                this.AddMirroredJoint("Hand", this.leftHand, this.rightHand, "Elbow", this.worldRight, this.worldForward, -90f, 0f, 0f, typeof(CapsuleCollider), 0.25f, 1.5f);
                this.AddJoint("Head", this.head, "Middle Spine", this.worldRight, this.worldForward, -40f, 25f, 25f, null, 1f, 1f);
            }
            else if (this.boneType == RagdollBoneType.Desmodus)
            {
                this.AddMirroredJoint("Hips", this.leftHips, this.rightHips, "Root", this.worldRight, this.worldForward, -20f, 70f, 30f, typeof(CapsuleCollider), 0.1f, 1.5f);
                this.AddJoint("Middle Spine", this.middleSpine, "Root", this.worldRight, this.worldForward, -20f, 20f, 10f, typeof(CapsuleCollider), 0.15f, 0.5f);
                this.AddJoint("Head", this.head, "Root", this.worldRight, this.worldForward, -40f, 25f, 25f, null, 1f, 1f);
            }
        }

        public void CreateRagdoll()
        {
            this.Cleanup();
            this.BuildCapsules();
            this.AddBreastColliders();
            this.AddHeadCollider();
            this.BuildBodies();
            this.BuildJoints();
            this.CalculateMass();
            this.CalculateSpringDampers();
        }

        private ZombineRagdoll.BoneInfo FindBone(string name)
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                if (boneInfo.name == name)
                {
                    return boneInfo;
                }
            }
            return null;
        }

        private void AddMirroredJoint(string name, Transform leftAnchor, Transform rightAnchor, string parent, Vector3 worldTwistAxis, Vector3 worldSwingAxis, float minLimit, float maxLimit, float swingLimit, Type colliderType, float radiusScale, float density)
        {
            this.AddJoint("Left " + name, leftAnchor, parent, worldTwistAxis, worldSwingAxis, minLimit, maxLimit, swingLimit, colliderType, radiusScale, density);
            this.AddJoint("Right " + name, rightAnchor, parent, worldTwistAxis, worldSwingAxis, minLimit, maxLimit, swingLimit, colliderType, radiusScale, density);
        }

        private void AddJoint(string name, Transform anchor, string parent, Vector3 worldTwistAxis, Vector3 worldSwingAxis, float minLimit, float maxLimit, float swingLimit, Type colliderType, float radiusScale, float density)
        {
            ZombineRagdoll.BoneInfo boneInfo = new ZombineRagdoll.BoneInfo();
            boneInfo.name = name;
            boneInfo.anchor = anchor;
            boneInfo.axis = worldTwistAxis;
            boneInfo.normalAxis = worldSwingAxis;
            boneInfo.minLimit = minLimit;
            boneInfo.maxLimit = maxLimit;
            boneInfo.swingLimit = swingLimit;
            boneInfo.density = density;
            boneInfo.colliderType = colliderType;
            boneInfo.radiusScale = radiusScale;
            if (this.FindBone(parent) != null)
            {
                boneInfo.parent = this.FindBone(parent);
            }
            else if (name.StartsWith("Left"))
            {
                boneInfo.parent = this.FindBone("Left " + parent);
            }
            else if (name.StartsWith("Right"))
            {
                boneInfo.parent = this.FindBone("Right " + parent);
            }
            boneInfo.parent.children.Add(boneInfo);
            this.bones.Add(boneInfo);
        }

        private void BuildCapsules()
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                if (boneInfo.colliderType == typeof(CapsuleCollider))
                {
                    int num = 0;
                    float num2 = 0f;
                    if (boneInfo.children.Count == 1)
                    {
                        ZombineRagdoll.BoneInfo boneInfo2 = (ZombineRagdoll.BoneInfo)boneInfo.children[0];
                        Vector3 position = boneInfo2.anchor.position;
                        ZombineRagdoll.CalculateDirection(boneInfo.anchor.InverseTransformPoint(position), out num, out num2);
                    }
                    if (num2 != 0f)
                    {
                        if (boneInfo.name == "Left Knee" || boneInfo.name == "Right Knee")
                        {
                            num2 += 0.3f * num2;
                        }
                        CapsuleCollider capsuleCollider = (CapsuleCollider)boneInfo.anchor.gameObject.AddComponent<CapsuleCollider>();
                        capsuleCollider.direction = num;
                        Vector3 zero = Vector3.zero;
                        zero[num] = num2 * 0.5f;
                        capsuleCollider.center = zero;
                        capsuleCollider.height = Mathf.Abs(num2);
                        capsuleCollider.radius = Mathf.Abs(num2 * boneInfo.radiusScale);
                        boneInfo.colliders = capsuleCollider;
                        boneInfo.anchor.gameObject.layer = LayerMask.NameToLayer("Ragdoll");
                    }
                }
            }
        }

        public void EnableRagdoll()
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                if (boneInfo.anchor)
                {
                    Component[] componentsInChildren = boneInfo.anchor.GetComponentsInChildren(typeof(Collider));
                    Component[] array = componentsInChildren;
                    for (int i = 0; i < array.Length; i++)
                    {
                        Collider collider = (Collider)array[i];
                        collider.enabled = true;
                    }
                }
            }
        }

        public void DisableRagdoll()
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                if (boneInfo.anchor)
                {
                    Component[] componentsInChildren = boneInfo.anchor.GetComponentsInChildren(typeof(Collider));
                    Component[] array = componentsInChildren;
                    for (int i = 0; i < array.Length; i++)
                    {
                        Collider collider = (Collider)array[i];
                        collider.enabled = false;
                    }
                }
            }
        }

        public void Cleanup()
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                if (boneInfo.anchor)
                {
                    boneInfo.joints = boneInfo.anchor.GetComponent<Joint>();
                    if (boneInfo.joints != null)
                    {
                        UnityEngine.Object.Destroy(boneInfo.joints);
                    }
                    boneInfo.bodies = boneInfo.anchor.GetComponent<Rigidbody>();
                    if (boneInfo.bodies != null)
                    {
                        UnityEngine.Object.Destroy(boneInfo.bodies);
                    }
                    if (boneInfo.colliders != null)
                    {
                        boneInfo.anchor.gameObject.layer = LayerMask.NameToLayer("Zombie");
                        UnityEngine.Object.Destroy(boneInfo.colliders);
                    }
                }
            }
            if (this.head != null && this.head.GetComponent<Collider>() != null)
            {
                UnityEngine.Object.Destroy(this.head.gameObject.GetComponent<Collider>());
                this.head.gameObject.layer = LayerMask.NameToLayer("Zombie");
            }
            if (this.middleSpine != null && this.middleSpine.GetComponent<BoxCollider>() != null)
            {
                UnityEngine.Object.Destroy(this.middleSpine.GetComponent<BoxCollider>());
                this.middleSpine.gameObject.layer = LayerMask.NameToLayer("Zombie");
            }
            if (this.root != null && this.root.GetComponent<BoxCollider>() != null)
            {
                UnityEngine.Object.Destroy(this.root.GetComponent<BoxCollider>());
            }
        }

        private void BuildBodies()
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                var rb = boneInfo.anchor.gameObject.AddComponent<Rigidbody>();
                if (rb)
                    rb.mass = boneInfo.density;
            }
        }

        private void BuildJoints()
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                if (boneInfo.parent != null)
                {
                    CharacterJoint characterJoint = (CharacterJoint)boneInfo.anchor.gameObject.AddComponent<CharacterJoint>();
                    boneInfo.joint = characterJoint;
                    characterJoint.axis = ZombineRagdoll.CalculateDirectionAxis(boneInfo.anchor.InverseTransformDirection(boneInfo.axis));
                    characterJoint.swingAxis = ZombineRagdoll.CalculateDirectionAxis(boneInfo.anchor.InverseTransformDirection(boneInfo.normalAxis));
                    characterJoint.anchor = Vector3.zero;
                    characterJoint.connectedBody = boneInfo.parent.anchor.GetComponent<Rigidbody>();
                    SoftJointLimit softJointLimit = default(SoftJointLimit);
                    softJointLimit.limit = boneInfo.minLimit;
                    characterJoint.lowTwistLimit = softJointLimit;
                    softJointLimit.limit = boneInfo.maxLimit;
                    characterJoint.highTwistLimit = softJointLimit;
                    softJointLimit.limit = boneInfo.swingLimit;
                    characterJoint.swing1Limit = softJointLimit;
                    softJointLimit.limit = 0f;
                    characterJoint.swing2Limit = softJointLimit;
                }
            }
        }

        private void CalculateMassRecurse(ZombineRagdoll.BoneInfo bone)
        {
            float num = bone.anchor.GetComponent<Rigidbody>().mass;
            foreach (ZombineRagdoll.BoneInfo boneInfo in bone.children)
            {
                this.CalculateMassRecurse(boneInfo);
                num += boneInfo.summedMass;
            }
            bone.summedMass = num;
        }

        private void CalculateMass()
        {
            this.CalculateMassRecurse(this.rootBone);
            float num = this.totalMass / this.rootBone.summedMass;
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                boneInfo.anchor.GetComponent<Rigidbody>().mass *= num;
            }
            this.CalculateMassRecurse(this.rootBone);
        }

        private JointDrive CalculateSpringDamper(float frequency, float damping, float mass)
        {
            return new JointDrive
            {
                positionSpring = 9f * frequency * frequency * mass,
                positionDamper = 4.5f * frequency * damping * mass
            };
        }

        private void CalculateSpringDampers()
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                if (boneInfo.joint)
                {

                    //boneInfo.joint.rotationDrive = CalculateSpringDamper(this.strength / 100f, 1f, boneInfo.summedMass);

                }
            }
        }

        private static void CalculateDirection(Vector3 point, out int direction, out float distance)
        {
            direction = 0;
            if (Mathf.Abs(point[1]) > Mathf.Abs(point[0]))
            {
                direction = 1;
            }
            if (Mathf.Abs(point[2]) > Mathf.Abs(point[direction]))
            {
                direction = 2;
            }
            distance = point[direction];
        }

        private static Vector3 CalculateDirectionAxis(Vector3 point)
        {
            int index = 0;
            float num;
            ZombineRagdoll.CalculateDirection(point, out index, out num);
            Vector3 zero = Vector3.zero;
            if (num > 0f)
            {
                zero[index] = 1f;
            }
            else
            {
                zero[index] = -1f;
            }
            return zero;
        }

        private static int SmallestComponent(Vector3 point)
        {
            int num = 0;
            if (Mathf.Abs(point[1]) < Mathf.Abs(point[0]))
            {
                num = 1;
            }
            if (Mathf.Abs(point[2]) < Mathf.Abs(point[num]))
            {
                num = 2;
            }
            return num;
        }

        private static int LargestComponent(Vector3 point)
        {
            int num = 0;
            if (Mathf.Abs(point[1]) > Mathf.Abs(point[0]))
            {
                num = 1;
            }
            if (Mathf.Abs(point[2]) > Mathf.Abs(point[num]))
            {
                num = 2;
            }
            return num;
        }

        private static int SecondLargestComponent(Vector3 point)
        {
            int num = ZombineRagdoll.SmallestComponent(point);
            int num2 = ZombineRagdoll.LargestComponent(point);
            if (num < num2)
            {
                int num3 = num2;
                num2 = num;
                num = num3;
            }
            if (num == 0 && num2 == 1)
            {
                return 2;
            }
            if (num == 0 && num2 == 2)
            {
                return 1;
            }
            return 0;
        }

        private Bounds Clip(Bounds bounds, Transform relativeTo, Transform clipTransform, bool below)
        {
            int index = ZombineRagdoll.LargestComponent(bounds.size);
            if (Vector3.Dot(this.worldUp, relativeTo.TransformPoint(bounds.max)) > Vector3.Dot(this.worldUp, relativeTo.TransformPoint(bounds.min)) == below)
            {
                Vector3 min = bounds.min;
                min[index] = relativeTo.InverseTransformPoint(clipTransform.position)[index];
                bounds.min = min;
            }
            else
            {
                Vector3 max = bounds.max;
                max[index] = relativeTo.InverseTransformPoint(clipTransform.position)[index];
                bounds.max = max;
            }
            return bounds;
        }

        private Bounds GetBreastBounds(Transform relativeTo)
        {
            Bounds result = default(Bounds);
            result.Encapsulate(relativeTo.InverseTransformPoint(this.leftHips.position));
            result.Encapsulate(relativeTo.InverseTransformPoint(this.rightHips.position));
            result.Encapsulate(relativeTo.InverseTransformPoint(this.leftArm.position));
            result.Encapsulate(relativeTo.InverseTransformPoint(this.rightArm.position));
            Vector3 size = result.size;
            size[ZombineRagdoll.SmallestComponent(result.size)] = size[ZombineRagdoll.LargestComponent(result.size)] / 2f;
            result.size = size;
            return result;
        }

        private void AddBreastColliders()
        {
            if (this.middleSpine != null && this.root != null)
            {
                Bounds bounds = this.Clip(this.GetBreastBounds(this.root), this.root, this.middleSpine, false);
                BoxCollider boxCollider = (BoxCollider)this.root.gameObject.AddComponent<BoxCollider>();
                boxCollider.center = bounds.center;
                boxCollider.size = bounds.size;
                if (this.boneType != RagdollBoneType.FlyHead)
                {
                    bounds = this.Clip(this.GetBreastBounds(this.middleSpine), this.middleSpine, this.middleSpine, true);
                    boxCollider = (BoxCollider)this.middleSpine.gameObject.AddComponent<BoxCollider>();
                    this.middleSpine.gameObject.layer = LayerMask.NameToLayer("Ragdoll");
                    boxCollider.center = bounds.center;
                    boxCollider.size = bounds.size;
                }
            }
            else
            {
                Bounds bounds2 = default(Bounds);
                bounds2.Encapsulate(this.root.InverseTransformPoint(this.leftHips.position));
                bounds2.Encapsulate(this.root.InverseTransformPoint(this.rightHips.position));
                bounds2.Encapsulate(this.root.InverseTransformPoint(this.leftArm.position));
                bounds2.Encapsulate(this.root.InverseTransformPoint(this.rightArm.position));
                Vector3 size = bounds2.size;
                size[ZombineRagdoll.SmallestComponent(bounds2.size)] = size[ZombineRagdoll.LargestComponent(bounds2.size)] / 2f;
                BoxCollider boxCollider2 = (BoxCollider)this.root.gameObject.AddComponent<BoxCollider>();
                boxCollider2.center = bounds2.center;
                boxCollider2.size = size;
            }
        }

        private void AddHeadCollider()
        {
            if (this.head.GetComponent<Collider>() != null)
            {
                UnityEngine.Object.Destroy(this.head.GetComponent<Collider>());
            }
            float num = Vector3.Distance(this.leftArm.transform.position, this.rightArm.transform.position);
            if (this.boneType == RagdollBoneType.FlyHead)
            {
                num *= 1.2f;
            }
            else
            {
                num /= 4f;
            }
            SphereCollider sphereCollider = (SphereCollider)this.head.gameObject.AddComponent<SphereCollider>();
            this.head.gameObject.layer = LayerMask.NameToLayer("Ragdoll");
            sphereCollider.radius = num;
            Vector3 zero = Vector3.zero;
            int index;
            float num2;
            ZombineRagdoll.CalculateDirection(this.head.InverseTransformPoint(this.root.position), out index, out num2);
            if (num2 > 0f)
            {
                zero[index] = -num;
            }
            else
            {
                zero[index] = num;
            }
            sphereCollider.center = zero;
        }

        public void ApplyRagdollEffect(RagdollEffect rdEffect)
        {
            if (rdEffect == null)
            {
                return;
            }
            if (this.root != null)
            {
                switch (rdEffect.type)
                {
                    case RagdollEffectType.Force:
                        this.root.GetComponent<Rigidbody>().AddForceAtPosition(rdEffect.force, rdEffect.forcePoint);
                        break;
                    case RagdollEffectType.Explosion:
                        this.root.GetComponent<Rigidbody>().AddExplosionForce(rdEffect.explosionForce, rdEffect.explosionPoint, rdEffect.explosionRadius);
                        break;
                    case RagdollEffectType.Mix:
                        this.root.GetComponent<Rigidbody>().AddForceAtPosition(rdEffect.force, rdEffect.forcePoint);
                        this.root.GetComponent<Rigidbody>().AddExplosionForce(rdEffect.explosionForce, rdEffect.explosionPoint, rdEffect.explosionRadius);
                        break;
                }
            }
        }

        public void BalanceGraviy(float gr)
        {
            foreach (ZombineRagdoll.BoneInfo boneInfo in this.bones)
            {
                if (!(boneInfo.anchor == null) && !(boneInfo.anchor.GetComponent<Rigidbody>() == null))
                {
                    boneInfo.anchor.GetComponent<Rigidbody>().AddForce(-Vector3.up * Physics.gravity.y * (1f - gr), ForceMode.Acceleration);
                }
            }
        }
        #endregion
    }
}
