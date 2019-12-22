- 追う側が逃げる側に当たったかどうかの判定
    - Runneragents.cs 内 OnCollisionEnter() 関数を使用して判定している。"chaser" または "boids" タグを持っているオブジェクトに当たった際に public にしてある hitcount の値を増やしている。
    - また同時にboidsフォルダ内 Boid.cs にて OnCollisionEnter() 関数で判定。 "Player" タグを持っているオブジェクトに当たった際に親の RollerAgent の持っている hitcount の値を増やしている。また、BoidParam 内の kamikaze が true の場合自身を破壊。

- 追う側が壁に当たったかどうかの判定
    - boidsフォルダ内 Boid.cs にて OnCollisionEnter() 関数で判定。 BoidParam 内の fragile が true の場合、"terrain" タグを持っているオブジェクトに当たった際に自身を破壊している。