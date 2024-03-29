﻿using System;

namespace SkyShoot.Service.Account
{
	[Serializable]
	public class AccountInfo
	{
		public string Login { get; set; }	// это rowKey в разделе partitionKey = "accounts"
		public string Password { get; set; }
		public string Email { get; set; }	// у приличных людей регистрация через эл.почту
		public string Info { get; set; }	// здесь в дальнейшем будет храниться доп.информация о аккаунте (очки бонусов, номер кредитки и т.д)
		// по идее нужно ещё добавить поле типа bool: true -- данный игрок сейчас залогинен и в игре, false -- игрок не в игре   
	}
}

