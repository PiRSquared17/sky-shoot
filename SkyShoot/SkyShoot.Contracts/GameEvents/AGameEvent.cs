﻿using System;
using System.Runtime.Serialization;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Utils;
using SkyShoot.Contracts.Weapon;

using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.GameEvents
{
	public enum EventType
	{
		NewObjectEvent,
		ObjectDirectionChangedEvent,
		ObjectShootEvent,
		ObjectDeletedEvent,
		WeaponChangedEvent,
		ObjectHealthChanged,
		RequestForEvents
	}

	[DataContract]
	[KnownType(typeof(NewObjectEvent))]
	[KnownType(typeof(ObjectDirectionChanged))]
	[KnownType(typeof(ObjectDeleted))]
	[KnownType(typeof(ObjectHealthChanged))]
	//[KnownType(typeof(BonusesChanged))]
	//[KnownType(typeof(WeaponChanged))]
	public abstract class AGameEvent
	{
		[DataMember]
		public long TimeStamp { get; private set; }

		[DataMember]
		public Guid? GameObjectId { get; private set; }

		[DataMember]
		public EventType Type;

		protected AGameEvent(Guid? id, long timeStamp)
		{
			GameObjectId = id;
			TimeStamp = timeStamp;
		}

		/// <summary>
		/// этот метод будет вызываться на клиенте при нахождении соответствующего объекта
		/// </summary>
		/// <param name="mob"></param>
		public abstract void UpdateMob(AGameObject mob);

		public override string ToString()
		{
			return String.Format("Time = {0}, Type = {1}", TimeStamp, Type);
		}
	}

	/// <summary>
	/// Вспомогательный класс, обозначающий пустое событие
	/// Используется клиентом для обозначения того,
	/// что нужно запросить с сервера GameEvent'ы
	/// </summary>
	public class RequestForEvents : AGameEvent
	{
		public RequestForEvents(Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			Type = EventType.RequestForEvents;
		}

		public override void UpdateMob(AGameObject mob)
		{
			throw new NotImplementedException();
		}
	}

	[DataContract]
	public class NewObjectEvent : AGameEvent
	{
		[DataMember]
		public AGameObject NewGameObject { get; private set; }

		public NewObjectEvent(AGameObject obj, long timeStamp)
			: base(obj.Id, timeStamp)
		{
			NewGameObject = GameObjectConverter.OneObject(obj);
			Type = EventType.NewObjectEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.Copy(NewGameObject);
		}
	}

	[DataContract]
	public class ObjectDirectionChanged : AGameEvent
	{
		[DataMember]
		public Vector2 NewRunDirection;

		public ObjectDirectionChanged(Vector2 direction, Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			NewRunDirection = direction;
			Type = EventType.ObjectDirectionChangedEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.RunVector = NewRunDirection;
		}
	}

	public class ObjectShootEvent : AGameEvent
	{
		public Vector2 ShootDirection;

		public ObjectShootEvent(Vector2 shootDirection, Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			ShootDirection = shootDirection;
			Type = EventType.ObjectShootEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			throw new NotImplementedException();
		}
	}

	[DataContract]
	public class ObjectDeleted : AGameEvent
	{
		public ObjectDeleted(Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			Type = EventType.ObjectDeletedEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.IsActive = false;
		}
	}

	[DataContract]
	public class ObjectHealthChanged : AGameEvent
	{
		[DataMember]
		public float Health;

		public ObjectHealthChanged(float newHp, Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			Health = newHp;
			Type = EventType.ObjectHealthChanged;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.HealthAmount = Health;
		}
	}

	/*[DataContract]*/
	public class WeaponChanged : AGameEvent
	{
		/*[DataMember] */
		public AWeapon Weapon;
		public WeaponType WeaponType;

		public WeaponChanged(AWeapon weapon, WeaponType weaponType, Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			Weapon = weapon;
			WeaponType = weaponType;
			Type = EventType.WeaponChangedEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.Weapon = Weapon;
		}
	}

	//[DataContract]
	//public class BonusesChanged : AGameEvent
	//{
	//  [DataMember]
	//  public AGameObject.EnumObjectType Bonuses;

	//  public BonusesChanged(Guid id, long timeStamp, AGameObject.EnumObjectType bonuses) :
	//    base(id, timeStamp)
	//  {
	//    Bonuses = bonuses;
	//  }

	//  public override void UpdateMob(AGameObject mob)
	//  {
	//    //todo //!! придумать что-то, ибо переносить в Mob бонусы нехорошо
	//  }
	//}
}