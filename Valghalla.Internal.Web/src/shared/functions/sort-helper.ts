export const DanishLocale = 'da';

const collatorCache: Record<string, Intl.Collator> = {};

function getCollator(localeCode: string, options: Intl.CollatorOptions): Intl.Collator {
	const cacheKey = localeCode + JSON.stringify(options);
	if (!collatorCache[cacheKey]) {
		collatorCache[cacheKey] = new Intl.Collator(localeCode, options);
	}
	return collatorCache[cacheKey];
}

function normalizeSortable(rawValue: string): string {
	if (!rawValue) return '';
	return rawValue.trim().replace(/^[-–—\s]+/, '');
}

const defaultOptions: Intl.CollatorOptions = { sensitivity: 'base', ignorePunctuation: true, usage: 'sort' };

export function compareStringsLocale(
	firstValue: string,
	secondValue: string,
	localeCode: string,
	options: Intl.CollatorOptions = defaultOptions
): number {
	const localeCollator = getCollator(localeCode, options);
	return localeCollator.compare(normalizeSortable(firstValue), normalizeSortable(secondValue));
}

export function sortArrayByNameLocale<T extends { name: string }>(
	entities: T[],
	localeCode: string,
	options: Intl.CollatorOptions = defaultOptions
): T[] {
	return entities.sort((l, r) => compareStringsLocale(l.name, r.name, localeCode, options));
}

export function sortArrayLocale<T>(
	entities: T[],
	valueSelector: (item: T) => string,
	localeCode: string,
	options: Intl.CollatorOptions = defaultOptions
): T[] {
	return entities.sort((l, r) => compareStringsLocale(valueSelector(l), valueSelector(r), localeCode, options));
}