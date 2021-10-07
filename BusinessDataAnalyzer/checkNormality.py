import os
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

col_list = ["ROA2012", "ROA2013", "ROA2014", "ROE2012", "ROE2013", "ROE2014"]
currentAnalyzedCol = "ROA2012"
df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/final_output.csv"), usecols=col_list, delimiter=';', encoding='utf8')

# remove nan and text values
analyzedFinancialValue = df[currentAnalyzedCol].apply(pd.to_numeric, errors='coerce').dropna()

# zero represents missing data
analyzedFinancialValueWithoutZeros = analyzedFinancialValue[analyzedFinancialValue!=0]

plt.hist(analyzedFinancialValueWithoutZeros, bins=np.arange(-3, 3, 0.001))

plt.title('Normal distribution check')
plt.xlabel('ROA')
plt.ylabel('Counts')
plt.ylim(top=800, bottom=0)

plt.show()
