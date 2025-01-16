# SPOOKY DART GAME
- Diego Rodríguez Martín - alu0101464992@ull.edu.es
- Juan Aday Siverio González - alu0101503950@ull.edu.es
- Raúl Álvarez Pérez - alu0101471136@ull.edu.es
- Sergio Nicolás Seguí - alu0101389936@ull.edu.es

## Menú

Antes de poder comenzar a jugar, debemos de pasar por un breve menú con ambiente tenebroso, al igual que en todo el juego. Incluso podemos llegar a ver un NPC, el cual es un monstruo que se va moviendo alrededor de toda la sala.

En este menú, para hacer uso de las funciones que nos proporciona CardBoard, vamos a tener que seleccionar las opciones mediante
movimientos del teléfono (o de la cabeza si está puesto en las gafas). Una vez encima de la opción que deseemos, vamos a ver que el retículo se expande, lo que nos da un indicio de que la opción se puede seleccionar.

### Selección de Jugadores

Al iniciar el juego, podremos mirar a la izquierda, observando así 4 carteles, cada uno con un número determinado de jugadores. Cada vez que seleccionemos uno diferente podremos ver como se actualiza el texto del canvas, dandonos a saber que se ha configurado correctamente el número de jugadores deseado. El número de jugadores por defecto es de 1.

Esta mecánica se implementará, obviamente una vez pasemos a la escena de juego, pudiendo jugar tantos jugadores como se desee. Para poder realizar esto, hemos empleado delegados y eventos. Para especificar, cuando pinchas en un cartel, este lanza un evento, al cual esta suscrita una función del script GameController. Dicha función tiene como objetivo modificar el número de jugadores en la mecánica de juego.

### Salir

Al igual que las otras opciones, para salir del juego vamos a pulsar sobre dicho cartel.

## Funcionamiento del Juego de los Dardos

El juego de los dardos implementado sigue una lógica por turnos, donde los jugadores deben lanzar dardos, acumular puntos y turnarse para continuar el juego hasta que se cumplan las condiciones definidas. A continuación, se detalla el funcionamiento del sistema.

![Interfaz Inteligente](gifInterfacesInteligentes.gif)

### Inicio del Juego
Cuando se inicia el juego, se inicializa un controlador general (`GameController`) que se encarga de gestionar todas las acciones principales. Este controlador establece las reglas del juego, organiza los turnos de los jugadores, administra las puntuaciones y lanza eventos que informan sobre los cambios de estado en el juego.

#### Configuración Inicial
1. **Número de Jugadores:** Se configura el número de jugadores mediante la variable `playersPlaying`. Este número puede actualizarse dinámicamente usando el método `UpdatePlayerNumber`.
2. **Puntuaciones:** Se inicializa una lista de puntos (`playersPoints`) donde cada jugador comienza con una puntuación de cero.
3. **Turnos y Estados:** Se establece al primer jugador como el jugador activo (`actualTurn = 0`) y se configura el estado inicial del juego en "Lanzando Dardos" (`GameState.ThrowingDarts`).

Cuando comienza una nueva partida, el método `InitializeNewGame` asegura que todos los valores se reinicien adecuadamente y notifica a los sistemas conectados mediante el evento `ResetGame`.

---

### Flujo del Juego
El juego se desarrolla a través de una serie de turnos, y cada turno está compuesto por las siguientes fases:

#### 1. Lanzamiento de Dardos
Cada jugador tiene la oportunidad de lanzar un número limitado de dardos por turno (en este caso, 2). Por cada lanzamiento, se incrementa la variable `dardosLanzados`.  
- **Límite de Dardos:** Si un jugador alcanza el límite de dardos, el estado cambia a "Siguiente Turno" (`GameState.NextTurn`) y se reinicia el contador de dardos para el próximo jugador.

#### 2. Cambio de Turno
Cuando un jugador termina su turno, se pasa al siguiente jugador. Esto se controla mediante la variable `actualTurn`, que se incrementa de manera cíclica utilizando una operación modular (`actualTurn = (actualTurn + 1) % playersPlaying`).  
- **Nueva Ronda:** Si el turno regresa al primer jugador (índice 0), se inicia una nueva ronda y se dispara el evento `NextRound`.  
- **Siguiente Jugador:** Si no es el inicio de una nueva ronda, se dispara el evento `NextPlayer`, indicando al sistema que el próximo jugador está listo para jugar.

#### 3. Acumulación de Puntos
Los puntos obtenidos en cada lanzamiento se registran en la lista `playersPoints`. Esto se realiza mediante el método `AddPointsToPlayer`, que suma los puntos al jugador actual.

---

### Eventos del Sistema
El controlador también está equipado con un sistema de eventos que permite a otros componentes reaccionar ante cambios en el juego. Estos eventos son:
- **`NextPlayer(int player):`** Se emite cuando el turno pasa al siguiente jugador, proporcionando su índice como parámetro.
- **`NextRound():`** Indica el inicio de una nueva ronda cuando todos los jugadores han completado un turno.
- **`ResetGame(int newPlayers):`** Reinicia el estado del juego y actualiza el número de jugadores.

---

### Escenarios de Uso
- **Inicio de una Nueva Partida:** Al iniciar una partida, el sistema asegura que todos los jugadores comiencen con las mismas condiciones. Esto incluye reiniciar las puntuaciones y configurar el turno inicial.
- **Control de Puntuaciones:** Los puntos acumulados por cada jugador son gestionados de forma independiente, permitiendo implementar diferentes modos de juego según la configuración de `GameMode`.
- **Transiciones Fluídas:** Gracias al sistema de eventos, otros elementos de la interfaz o del sistema de juego (como tableros de puntuación o animaciones) pueden reaccionar automáticamente a los cambios en el estado del juego.


### Puntuación

Cuando entramos en la sala de juego, podemos observar que a nuestra derecha se encuentra un tablero con 3 filas para cada juagdor: la puntuación de la ronda actual, la puntuación total y el turno. Este último estará marcado con una X de color rojo en la columna del jugador al que le corresponde tirar.

### Dardos Enganchados

Una vez el jugador haya tirado todos los dardos que le corresponden en su ronda, debemos de eliminarlos de la diana para que no molesten al siguiente jugador al tirar. Esto lo haremos contando los dardos lanzados y, cuando llegue a 3, eliminaremos los dardos que son hijos de la diana, es decir los que ya han sido lanzados. Esto lo haremos de esta manera para evitar que se nos eliminen todos los dardos de la partida, y así no perder el dardo con el que lanza el jugador.

### Límite de puntos

Como bien sabemos, en el juego clásico de los dardos hay que llegar a una puntuación establecida previamente (301, 501, 701, etc.) de tal manera que el dardo que llegue a esa puntuación no se pase de dicha puntuación. Si este se pasa, debemos de eliminar la puntuación que ha conseguido el jugador en dicha ronda. Esto lo hacemos invocando a un evento con la puntuación a la que se debe de establecer la puntuacion total del jugador (es decir la misma que la acumulada en la ronda anterior, ya que en esta no cuentan los puntos).

## Funcionamiento de la Diana

Este script implementa el comportamiento y las funcionalidades de una diana en un juego de dardos. Se encarga de detectar impactos de los dardos, calcular las puntuaciones según la posición del impacto y gestionar la interacción con el sistema principal del juego.

### Componentes Principales
El script utiliza varios elementos clave para modelar la diana y gestionar los impactos de los dardos:

1. **Centro y Radios de la Diana:**
   - `dartboardCenter`: Define el centro de la diana en coordenadas locales.
   - Varios radios (`bullseyeRadius`, `outerBullseyeRadius`, etc.) delimitan las distintas zonas de puntuación de la diana, como el Bullseye, el Triple, el Doble y el perímetro.

2. **Puntuaciones por Sectores:**
   - La diana está dividida en 20 sectores con puntuaciones definidas en el arreglo `sectorScores`.
   - Los sectores se calculan en función del ángulo del impacto, ajustando el sistema angular para coincidir con los sectores típicos de una diana.

3. **Dardos y Eventos:**
   - `darts`: Lista que almacena las posiciones de los dardos lanzados.
   - **Eventos:** El evento `DartImpacted` notifica a otros sistemas cuando un dardo impacta la diana, enviando la puntuación obtenida.

4. **Interfaz Visual:**
   - `showActualLaunch`: Array de elementos de texto que muestra la puntuación del impacto más reciente.


### 1. **Inicialización**
- En el método `Awake`, se establece el centro de la diana (`dartboardCenter`) como el origen `(0,0)`.
- El método `Start` calcula las puntuaciones iniciales para los dardos almacenados (si los hay) mediante `CalculateScores`.

### 2. **Gestión de Impactos**
Cuando un dardo impacta la diana, se desencadena el método `OnTriggerEnter`, que realiza las siguientes acciones:

1. **Verificar el Objeto Impactado:**
   - Comprueba si el objeto que colisionó tiene la etiqueta `"DART"`.
   - Aplica un límite de tiempo entre impactos para evitar detecciones consecutivas demasiado rápidas (`delayBetweenLaunchs`).

2. **Fijar el Dardo en la Diana:**
   - El dardo se agrega como hijo de la diana, asegurando que permanezca en su posición de impacto.
   - Se desactiva la física del dardo (`isKinematic = true`) para que quede "clavado" en la diana.

3. **Calcular la Puntuación:**
   - La posición de impacto del dardo se evalúa mediante el método `GetScore`, que calcula la distancia al centro y determina el sector correspondiente.
   - Las puntuaciones se ajustan según las zonas especiales (Bullseye, Triple, Doble) o se considera un fallo si está fuera de la diana.

4. **Actualizar el Sistema de Puntos:**
   - Se suman los puntos al jugador actual a través de `GameController.Instance.AddPointsToPlayer`.
   - Se dispara el evento `DartImpacted` para informar a otros sistemas sobre la puntuación obtenida.
   - Se actualizan los elementos visuales (`showActualLaunch`) para reflejar la puntuación del último impacto.

### 3. **Cálculo de Puntuaciones**
El método `GetScore` evalúa la posición de un dardo y asigna la puntuación correspondiente:
- **Centro de la Diana:**
  - Bullseye: 50 puntos.
  - Outer Bullseye: 25 puntos.
- **Sectores y Multiplicadores:**
  - La posición angular del impacto determina el sector, mientras que la distancia determina si se aplica un multiplicador (Triple o Doble).
- **Fuera de la Diana:**
  - Si el impacto está fuera del radio total (`dartboardRadius`), la puntuación es 0.

### 4. **Representación Visual**
El método `OnDrawGizmos` dibuja las distintas zonas de la diana en el editor de Unity, facilitando la depuración y visualización de los radios.

---

## Eventos del Sistema
El script incluye un sistema de eventos para comunicar impactos al resto del juego:
- **`DartImpacted(int points):`** Se emite cuando un dardo impacta la diana, proporcionando la puntuación obtenida como parámetro.

---

## Resumen del Flujo
1. Un dardo impacta la diana y activa el trigger `OnTriggerEnter`.
2. El sistema calcula la posición del impacto y determina la puntuación.
3. El dardo se fija en la diana y se actualizan las puntuaciones del jugador.
4. Se notifica el impacto al resto del sistema mediante eventos.
5. La puntuación se muestra en los elementos visuales correspondientes.
