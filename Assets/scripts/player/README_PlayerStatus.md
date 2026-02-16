# Sistema de Status do Player

## Visão Geral
Sistema centralizado que gerencia todos os status do jogador em um único componente `PlayerStatus.cs`.

## Status Disponíveis

### 1. **Stamina** (Resistência)
- **Valor Inicial**: 100 (máximo)
- **Uso**: Consumida ao correr (Sprint)
- **Regeneração**: Automática quando não está correndo
- **Cooldown**: Quando a stamina chega a 0, entra em cooldown e só volta a funcionar quando regenerar completamente

### 2. **Vida** (Health)
- **Valor Inicial**: 100 (máximo)
- **Uso**: Diminui ao receber dano
- **Métodos**:
  - `TakeDamage(float damage)` - Recebe dano
  - `Heal(float amount)` - Recupera vida
  - `Die()` - Chamado automaticamente quando vida chega a 0

### 3. **Sono** (Sleep)
- **Valor Inicial**: 0
- **Uso**: Aumenta com o tempo, afeta o jogador quando muito alto
- **Métodos**:
  - `IncreaseSono(float amount)` - Aumenta sono
  - `DecreaseSono(float amount)` - Diminui sono (dormir, café, etc)

### 4. **Pânico** (Panic)
- **Valor Inicial**: 0
- **Uso**: Aumenta em situações de perigo/terror
- **Métodos**:
  - `IncreasePanico(float amount)` - Aumenta pânico
  - `DecreasePanico(float amount)` - Diminui pânico (calmantes, áreas seguras, etc)

### 5. **Alucinação** (Hallucination)
- **Valor Inicial**: 0
- **Uso**: Aumenta com eventos sobrenaturais, falta de sono, etc
- **Métodos**:
  - `IncreaseAlucinacao(float amount)` - Aumenta alucinação
  - `DecreaseAlucinacao(float amount)` - Diminui alucinação

## Como Usar

### Configuração no Unity
1. Adicione o componente `PlayerStatus` ao GameObject do Player
2. O `FirstPersonController` já está configurado para usar o `PlayerStatus`
3. Todos os valores máximos podem ser ajustados no Inspector

### Acessando de Outros Scripts
```csharp
// Obter referência ao PlayerStatus
PlayerStatus playerStatus = GetComponent<PlayerStatus>();

// Exemplos de uso:
playerStatus.TakeDamage(10f); // Causa 10 de dano
playerStatus.Heal(20f); // Recupera 20 de vida
playerStatus.IncreasePanico(5f); // Aumenta pânico em 5
playerStatus.DecreaseSono(30f); // Diminui sono em 30
playerStatus.IncreaseAlucinacao(15f); // Aumenta alucinação em 15
```

## Mudanças Realizadas

### FirstPersonController.cs
- **Removido**: Variáveis `Stamina`, `MaxStamina`, `Cooldown_Stamina`
- **Adicionado**: Referência ao `PlayerStatus`
- **Modificado**: Método `Sprint()` agora usa `playerStatus.CanUseStamina()`, `ConsumeStamina()` e `RegenerateStamina()`
- **Mantido**: Toda a lógica de movimento e sprint funciona exatamente como antes

### PlayerStatus.cs (NOVO)
- Gerencia todos os 5 status do player
- Métodos públicos para modificar cada status
- Sistema de cooldown da stamina mantido
- Regeneração automática da stamina no Update()
- Método `Die()` para quando a vida chega a 0

## Próximos Passos Sugeridos
1. Criar UI para exibir os status
2. Implementar efeitos visuais baseados nos status (ex: tela tremendo com pânico alto)
3. Adicionar sistema de aumento gradual de sono com o tempo
4. Criar eventos que aumentam pânico e alucinação
5. Implementar mecânicas de gameplay baseadas nos status (ex: alucinações visuais quando alucinação está alta)
